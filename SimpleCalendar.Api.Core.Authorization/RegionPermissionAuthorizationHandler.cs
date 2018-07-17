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
        private readonly IRegionMembershipCache _regionMembershipCache;
        private readonly IRegionPermissionResolver _regionPermissionResolver;

        public RegionPermissionAuthorizationHandler(
            IRegionRoleCache regionRoleCache,
            IRegionMembershipCache regionMembershipCache,
            IRegionPermissionResolver regionPermissionResolver)
        {
            _regionRoleCache = regionRoleCache;
            _regionMembershipCache = regionMembershipCache;
            _regionPermissionResolver = regionPermissionResolver;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, RegionPermissionRequirement requirement, RegionEntity resource)
        {
            var roles = await _regionRoleCache.ListAsync();
            var memberships = await _regionMembershipCache.ListRegionMembershipsAsync(context.User.GetUserEmail());

            if (_regionPermissionResolver.HasPermission(requirement.Permission, resource, roles, memberships))
            {
                context.Succeed(requirement);
            }
        }
    }
}
