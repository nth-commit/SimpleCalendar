using MoreLinq;
using SimpleCalendar.Api.Core.Data;
using SimpleCalendar.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace SimpleCalendar.Api.Core.Authorization
{
    public class RegionPermissionResolver : IRegionPermissionResolver
    {
        public bool HasPermission(
            ClaimsPrincipal user,
            RegionEntity region,
            RegionPermission permission,
            IEnumerable<RegionRoleEntity> regionRoles)
        {
            var regionRolesById = regionRoles.ToDictionary(r => r.Id);
            var regionMembershipsByRegionId = user.GetRegionMembershipRoles().ToLookup(r => r.RegionId);
            return HasPermission(permission, region, regionRolesById, regionMembershipsByRegionId, isParentOfTargetRegion: false);
        }

        private bool HasPermission(
            RegionPermission permission,
            RegionEntity region,
            IDictionary<string, RegionRoleEntity> regionRolesById,
            ILookup<string, ClaimsExtensions.RegionMembershipRoleClaimValue> regionMembershipsByRegionId,
            bool isParentOfTargetRegion)
        {
            var regionRoles = regionMembershipsByRegionId[region.Id]
                .DistinctBy(r => r.RegionRoleId)
                .Select(r => regionRolesById[r.RegionRoleId]);

            if (HasPermission(permission, regionRoles, r => r.Permissions) ||
                (isParentOfTargetRegion && HasPermission(permission, regionRoles, r => r.ChildPermissions)))
            {
                return true;
            }

            return region.Id == Data.Constants.RootRegionId ?
                false :
                HasPermission(permission, region.Parent, regionRolesById, regionMembershipsByRegionId, isParentOfTargetRegion: true);
        }

        private bool HasPermission(
            RegionPermission permission,
            IEnumerable<RegionRoleEntity> regionRoles,
            Func<RegionRoleEntity, RegionPermission> permissionsSelector)
        {
            return regionRoles.Any(r => permissionsSelector(r).HasFlag(permission));
        }
    }
}
