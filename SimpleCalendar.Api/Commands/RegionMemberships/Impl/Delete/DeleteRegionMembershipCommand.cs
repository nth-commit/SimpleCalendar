using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SimpleCalendar.Api.Core.Data;
using SimpleCalendar.Api.Services;
using SimpleCalendar.Utility.Authorization;

namespace SimpleCalendar.Api.Commands.RegionMemberships.Impl.Delete
{
    public class DeleteRegionMembershipCommand : IDeleteRegionMembershipCommand
    {
        private readonly IUserAuthorizationService _authorizationService;
        private readonly CoreDbContext _coreDbContext;
        private readonly IRegionCache _regionCache;

        public DeleteRegionMembershipCommand(
            IUserAuthorizationService authorizationService,
            CoreDbContext coreDbContext,
            IRegionCache regionCache)
        {
            _authorizationService = authorizationService;
            _coreDbContext = coreDbContext;
            _regionCache = regionCache;
        }

        public async Task<IActionResult> InvokeAsync(ActionContext context, string id)
        {
            var entity = await _coreDbContext.RegionMemberships.FindAsync(id);
            if (entity == null)
            {
                return new NotFoundResult();
            }

            var region = await _regionCache.GetRegionAsync(entity.RegionId);
            var canDelete = await _authorizationService.CanDeleteMembershipsAsync(region, entity.RegionRoleId, entity.UserEmail);
            if (!canDelete)
            {
                return new UnauthorizedResult();
            }

            _coreDbContext.RegionMemberships.Remove(entity);
            await _coreDbContext.SaveChangesAsync();

            return new NoContentResult();
        }
    }
}
