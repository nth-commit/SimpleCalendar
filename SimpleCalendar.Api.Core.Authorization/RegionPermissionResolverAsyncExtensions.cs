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

        public static bool HasPermission(
            this IRegionPermissionResolver regionPermissionResolver,
            ClaimsPrincipal user,
            RegionEntity region,
            RegionPermission permission,
            IEnumerable<RegionRoleEntity> regionRoles)
        {
            if (!TryGetUserEmail(user, out string userEmail))
            {
                return false;
            }

            return regionPermissionResolver.HasPermission(
                user,
                region,
                permission,
                regionRoles);
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
