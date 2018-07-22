using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleCalendar.Api.Core.Data;
using SimpleCalendar.Api.Models;
using SimpleCalendar.Framework;
using SimpleCalendar.Utility.Authorization;

namespace SimpleCalendar.Api.Commands.RegionMemberships.Impl.Query
{
    public class QueryRegionMembershipCommand : IQueryRegionMembershipCommand
    {
        private readonly IUserAuthorizationService _authorizationService;
        private readonly CoreDbContext _coreDbContext;

        public QueryRegionMembershipCommand(
            IUserAuthorizationService authorizationService,
            CoreDbContext coreDbContext)
        {
            _authorizationService = authorizationService;
            _coreDbContext = coreDbContext;
        }

        public async Task<IActionResult> InvokeAsync(ActionContext context, string regionId, string userEmail)
        {
            var isRegionQuery = !string.IsNullOrWhiteSpace(regionId);
            var isUserQuery = !string.IsNullOrEmpty(userEmail);

            if (!isUserQuery && !isRegionQuery)
            {
                var canQuery = context.HttpContext.User.IsAdministratorOrSuperAdministrator();
                if (!canQuery)
                {
                    return new UnauthorizedResult();
                }
            }

            if (isUserQuery)
            {
                var user = context.HttpContext.User;
                var canQueryUser = userEmail == user.GetUserEmail() || user.IsAdministratorOrSuperAdministrator();
                if (!canQueryUser)
                {
                    return new UnauthorizedResult();
                }
            }

            var query = _coreDbContext.RegionMemberships.AsQueryable();

            RegionEntity region = null;
            if (isRegionQuery)
            {
                region = await _coreDbContext.GetRegionByCodesAsync(regionId);
                if (region == null)
                {
                    context.ModelState.AddModelError(nameof(regionId), "Region could not be found");
                    return new BadRequestObjectResult(context.ModelState);
                }

                var canQueryRegion = await _authorizationService.CanQueryMembershipsAsync(region);
                if (!canQueryRegion)
                {
                    return new UnauthorizedResult();
                }

                query = query.Where(r => r.RegionId == region.Id);
            }

            if (!string.IsNullOrWhiteSpace(userEmail))
            {
                query = query.Where(r => r.UserEmail == userEmail);
            }

            var entities = await query.ToListAsync();
            if (!entities.Any())
            {
                return new OkObjectResult(Enumerable.Empty<RegionMembership>());
            }

            var regionIds = entities
                .Select(e => e.RegionId)
                .Distinct();

            Dictionary<string, RegionEntity> regionsById = null;
            if (isRegionQuery)
            {
                regionsById = regionIds.ToDictionary(r => r, r => region);
            }
            else
            {
                var regions = await Task.WhenAll(regionIds.Select(r => _coreDbContext.GetRegionByIdAsync(r)));
                regionsById = regions.ToDictionary(r => r.Id);
            }

            return new OkObjectResult(entities.Select(e => new RegionMembership()
            {
                Id = e.Id,
                RegionId = regionsById[e.RegionId].GetId(),
                UserEmail = e.UserEmail,
                RegionRoleId = e.RegionRoleId,
                Permissions = new RegionMembershipAuthorization()
                {
                    CanDelete = _authorizationService.CanDeleteMembershipsAsync(
                        regionsById[e.RegionId],
                        e.RegionRoleId,
                        e.UserEmail).Result
                }
            }));
        }
    }
}
