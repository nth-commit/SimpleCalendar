using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleCalendar.Api.Core.Data;
using SimpleCalendar.Api.Models;
using SimpleCalendar.Utility.Authorization;

namespace SimpleCalendar.Api.Commands.RegionMemberships.Impl.Create
{
    public class CreateRegionMembershipCommand : ICreateRegionMembershipCommand
    {
        private readonly IUserAuthorizationService _authorizationService;
        private readonly CoreDbContext _coreDbContext;

        public CreateRegionMembershipCommand(
            IUserAuthorizationService authorizationService,
            CoreDbContext coreDbContext)
        {
            _authorizationService = authorizationService;
            _coreDbContext = coreDbContext;
        }

        public async Task<IActionResult> InvokeAsync(ActionContext context, RegionMembershipCreate create)
        {
            if (!context.ModelState.IsValid)
            {
                return new BadRequestObjectResult(context.ModelState);
            }

            var region = await _coreDbContext.GetRegionByCodesAsync(create.RegionId);
            if (region == null)
            {
                context.ModelState.AddModelError(nameof(RegionMembershipCreate.RegionId), "Region not found");
                return new BadRequestObjectResult(context.ModelState);
            }

            var canCreate = await _authorizationService.CanCreateMembershipAsync(region, create.RegionRoleId);
            if (!canCreate)
            {
                return new UnauthorizedResult();
            }

            var membershipExists = await _coreDbContext.RegionMemberships
                .Where(r => r.RegionId == region.Id)
                .Where(r => r.UserEmail == create.UserEmail)
                .AnyAsync();

            if (membershipExists)
            {
                context.ModelState.AddModelError(
                    $"{nameof(RegionMembershipCreate.RegionId)}+{nameof(RegionMembershipCreate.UserEmail)}",
                    "User already has a membership for this region");
                return new BadRequestObjectResult(context.ModelState);
            }

            var regionRoles = await _coreDbContext.RegionRoles.ToListAsync();
            if (!regionRoles.Any(rr => rr.Id == create.RegionRoleId))
            {
                context.ModelState.AddModelError(
                    nameof(RegionMembershipCreate.RegionRoleId),
                    "Region role does not exist");
                return new BadRequestObjectResult(context.ModelState);
            }

            var result = await _coreDbContext.RegionMemberships.AddAsync(new RegionMembershipEntity()
            {
                UserEmail = create.UserEmail,
                RegionId = region.Id,
                RegionRoleId = create.RegionRoleId
            });

            return new CreatedResult(
                $"regionmemberships/{result.Entity.Id}",
                new RegionMembership()
                {
                    Id = result.Entity.Id,
                    UserId = result.Entity.UserEmail,
                    RegionId = create.RegionId,
                    RegionRoleId = create.RegionRoleId,
                    Permissions = new RegionMembershipAuthorization()
                    {
                        CanDelete = _authorizationService.CanDeleteMembershipsAsync(region).Result
                    }
                });
        }
    }
}
