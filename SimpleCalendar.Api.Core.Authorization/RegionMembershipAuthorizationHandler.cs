using Microsoft.AspNetCore.Authorization;
using SimpleCalendar.Api.Core.Data;
using SimpleCalendar.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCalendar.Api.Core.Authorization
{
    public class RegionMembershipAuthorizationHandler : AuthorizationHandler<RegionMembershipRequirement>
    {
        private readonly IRegionCache _regionCache;
        private readonly IRegionRoleCache _regionRoleCache;
        private readonly IRegionPermissionResolver _regionPermissionResolver;

        public RegionMembershipAuthorizationHandler(
            IRegionCache regionCache,
            IRegionRoleCache regionRoleCache,
            IRegionPermissionResolver regionPermissionResolver)
        {
            _regionCache = regionCache;
            _regionRoleCache = regionRoleCache;
            _regionPermissionResolver = regionPermissionResolver;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            RegionMembershipRequirement requirement)
        {
            if (requirement is RegionMembershipRequirement.QueryRequirement queryRequirement &&
                await CanQueryRegionMemberships(context, queryRequirement))
            {
                context.Succeed(requirement);
            }
        }

        private async Task<bool> CanQueryRegionMemberships(
            AuthorizationHandlerContext context,
            RegionMembershipRequirement.QueryRequirement queryRequirement)
        {
            if (!context.User.Identity.IsAuthenticated)
            {
                return false;
            }

            if (context.User.GetUserEmail() == queryRequirement.Email)
            {
                return true;
            }

            var regionTask = _regionCache.GetRegionByCodesAsync(queryRequirement.RegionId);
            var regionRolesTask = _regionRoleCache.ListAsync();
            await Task.WhenAll(regionTask, regionRolesTask);

            var region = regionTask.Result;
            if (region == null)
            {
                return true;
            }

            return _regionPermissionResolver.HasPermission(
                context.User,
                region,
                RegionPermission.Memberships_Read,
                regionRolesTask.Result);
        }
    }
}
