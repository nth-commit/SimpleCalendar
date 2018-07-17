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
            ClaimsPrincipal user,
            RegionEntity region,
            RegionPermission permission,
            IRegionRoleCache regionRoleCache)
                => regionPermissionResolver.HasPermissionAsync(
                    user,
                    region,
                    permission,
                    new Lazy<Task<IEnumerable<RegionRoleEntity>>>(
                        () => regionRoleCache.ListAsync()));

        public static async Task<bool> HasPermissionAsync(
            this IRegionPermissionResolver regionPermissionResolver,
            ClaimsPrincipal user,
            RegionEntity region,
            RegionPermission permission,
            Lazy<Task<IEnumerable<RegionRoleEntity>>> lazyRegionRolesTask)
        {
            if (!TryGetUserEmail(user, out string userEmail))
            {
                return false;
            }

            return regionPermissionResolver.HasPermission(
                user,
                region,
                permission,
                await lazyRegionRolesTask.Value);
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
