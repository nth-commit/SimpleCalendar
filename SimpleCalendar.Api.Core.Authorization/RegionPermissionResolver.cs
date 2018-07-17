using MoreLinq;
using SimpleCalendar.Api.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleCalendar.Api.Core.Authorization
{
    public class RegionPermissionResolver : IRegionPermissionResolver
    {
        public bool HasPermission(
            RegionPermission permission,
            RegionEntity region,
            IEnumerable<RegionRoleEntity> regionRoles,
            IEnumerable<RegionMembershipEntity> regionMemberships)
        {
            var regionRolesById = regionRoles.ToDictionary(r => r.Id);
            var regionMembershipsByRegionId = regionMemberships.ToLookup(r => r.RegionId);
            return HasPermission(permission, region, regionRolesById, regionMembershipsByRegionId, isParentOfTargetRegion: false);
        }

        private bool HasPermission(
            RegionPermission permission,
            RegionEntity region,
            IDictionary<string, RegionRoleEntity> regionRolesById,
            ILookup<string, RegionMembershipEntity> regionMembershipsByRegionId,
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

            return region.Id == Constants.RootRegionId ?
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
