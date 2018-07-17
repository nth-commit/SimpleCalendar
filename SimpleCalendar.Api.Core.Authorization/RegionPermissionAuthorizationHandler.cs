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
        private readonly IRegionRoleCache _regionRoleCache;
        private readonly IRegionPermissionResolver _regionPermissionResolver;

        public RegionPermissionAuthorizationHandler(
            IRegionRoleCache regionRoleCache,
            IRegionPermissionResolver regionPermissionResolver)
        {
            _regionRoleCache = regionRoleCache;
            _regionPermissionResolver = regionPermissionResolver;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, RegionPermissionRequirement requirement, RegionEntity resource)
        {
            var roles = await _regionRoleCache.ListAsync();
            if (_regionPermissionResolver.HasPermission(context.User, resource, requirement.Permission, roles))
            {
                context.Succeed(requirement);
            }
        }
    }
}
