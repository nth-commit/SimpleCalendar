using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SimpleCalendar.Api.Core.Data;
using SimpleCalendar.Utility.Authorization;

namespace SimpleCalendar.Api.Commands.RegionMemberships.Impl
{
    public class DeleteRegionMembershipCommand : IDeleteRegionMembershipCommand
    {
        private readonly IUserAuthorizationService _authorizationService;
        private readonly CoreDbContext _coreDbContext;

        public DeleteRegionMembershipCommand(
            IUserAuthorizationService authorizationService,
            CoreDbContext coreDbContext)
        {
            _authorizationService = authorizationService;
            _coreDbContext = coreDbContext;
        }

        public async Task<IActionResult> InvokeAsync(ActionContext context, string id)
        {
            var entity = await _coreDbContext.RegionMemberships.FindAsync(id);
            if (entity == null)
            {
                return new NotFoundResult();
            }

            var region = await _coreDbContext.GetRegionByIdAsync(entity.RegionId);
            var canDelete = await _authorizationService.CanDeleteMembershipsAsync(region);
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
