using Microsoft.AspNetCore.Authorization;
using SimpleCalendar.Api.Core.Data;
using SimpleCalendar.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCalendar.Api.Core.Authorization
{
    public class RegionPermissionAuthorizationHandler : AuthorizationHandler<RegionPermissionRequirement, RegionEntity>
    {
        private readonly IRegionRolesAccessor _regionRolesAccessor;
        private readonly IRegionPermissionResolver _regionPermissionResolver;

        public RegionPermissionAuthorizationHandler(
            IRegionRolesAccessor regionRolesAccessor,
            IRegionPermissionResolver regionPermissionResolver)
        {
            _regionRolesAccessor = regionRolesAccessor;
            _regionPermissionResolver = regionPermissionResolver;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RegionPermissionRequirement requirement, RegionEntity resource)
        {
            var roles = _regionRolesAccessor.RegionRoles;
            if (_regionPermissionResolver.HasPermission(context.User, resource, requirement.Permission, roles))
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}
