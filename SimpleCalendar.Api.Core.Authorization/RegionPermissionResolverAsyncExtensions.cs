using SimpleCalendar.Api.Core.Data;
using SimpleCalendar.Framework;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCalendar.Api.Core.Authorization
{
    public static class RegionPermissionResolverAsyncExtensions
    {
        public static Task<bool> HasPermissionAsync(
            this IRegionPermissionResolver regionPermissionResolver,
            RegionEntity region,
            RegionPermission permission,
            ClaimsPrincipal user,
            IRegionRoleCache regionRoleCache,
            IRegionMembershipCache regionMembershipCache)
                => regionPermissionResolver.HasPermissionAsync(
                    region,
                    permission,
                    user,
                    new Lazy<Task<IEnumerable<RegionRoleEntity>>>(
                        () => regionRoleCache.ListAsync()),
                    new Lazy<Task<IEnumerable<RegionMembershipEntity>>>(
                        () => regionMembershipCache.ListRegionMembershipsAsync(user.GetUserEmail())));

        public static async Task<bool> HasPermissionAsync(
            this IRegionPermissionResolver regionPermissionResolver,
            RegionEntity region,
            RegionPermission permission,
            ClaimsPrincipal user,
            Lazy<Task<IEnumerable<RegionRoleEntity>>> lazyRegionRolesTask,
            Lazy<Task<IEnumerable<RegionMembershipEntity>>> lazyRegionMembershipsTask)
        {
            if (!TryGetUserEmail(user, out string userEmail))
            {
                return false;
            }

            var regionRolesTask = lazyRegionRolesTask.Value;
            var regionMembershipsTask = lazyRegionMembershipsTask.Value;
            await Task.WhenAll(regionRolesTask, regionMembershipsTask);

            return regionPermissionResolver.HasPermission(
                region,
                permission,
                new RegionPermissionResolutionContext(user, regionRolesTask.Result, regionMembershipsTask.Result));
        }

        private static bool TryGetUserEmail(ClaimsPrincipal user, out string userEmail)
        {
            if (user.Identity.IsAuthenticated)
            {
                userEmail = user.GetUserEmail();
                return true;
            }

            userEmail = null;
            return false;
        }
    }
}
