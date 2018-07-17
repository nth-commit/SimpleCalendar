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
        private readonly IRegionMembershipCache _regionMembershipCache;
        private readonly IRegionPermissionResolver _regionPermissionResolver;

        public RegionMembershipAuthorizationHandler(
            IRegionCache regionCache,
            IRegionRoleCache regionRoleCache,
            IRegionMembershipCache regionMembershipCache,
            IRegionPermissionResolver regionPermissionResolver)
        {
            _regionCache = regionCache;
            _regionRoleCache = regionRoleCache;
            _regionMembershipCache = regionMembershipCache;
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
            var regionMembershipsTask = _regionMembershipCache.ListRegionMembershipsAsync(context.User.GetUserEmail());
            await Task.WhenAll(regionTask, regionRolesTask, regionMembershipsTask);

            var region = regionTask.Result;
            if (region == null)
            {
                return true;
            }

            return _regionPermissionResolver.HasPermission(
                RegionPermission.Memberships_Read,
                region,
                regionRolesTask.Result,
                regionMembershipsTask.Result);
        }
    }
}
