using SimpleCalendar.Api.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace SimpleCalendar.Api.Core.Authorization
{
    public interface IRegionPermissionResolver
    {
        bool HasPermission(
            RegionPermission permission,
            RegionEntity region,
            IEnumerable<RegionRoleEntity> regionRoles,
            IEnumerable<RegionMembershipEntity> regionMemberships);
    }

    public class RegionPermissionResolutionContext
    {
        public RegionPermissionResolutionContext(
            ClaimsPrincipal user = null,
            IEnumerable<RegionRoleEntity> regionRoles = null,
            IEnumerable<RegionMembershipEntity> regionMemberships = null)
        {
            User = user ?? new ClaimsPrincipal(new ClaimsIdentity());
            RegionRoles = regionRoles ?? Enumerable.Empty<RegionRoleEntity>();
            RegionMemberships = regionMemberships ?? Enumerable.Empty<RegionMembershipEntity>();
        }

        public ClaimsPrincipal User { get; }

        public IEnumerable<RegionRoleEntity> RegionRoles { get; }

        public IEnumerable<RegionMembershipEntity> RegionMemberships { get; }


    }

    public static class RegionPermissionResolverExtensions
    {
        public static bool HasPermission(
            this IRegionPermissionResolver regionPermissionResolver,
            RegionEntity region,
            RegionPermission permission,
            RegionPermissionResolutionContext context) =>
                regionPermissionResolver.HasPermission(
                    permission,
                    region,
                    context.RegionRoles,
                    context.RegionMemberships);
    }
}
