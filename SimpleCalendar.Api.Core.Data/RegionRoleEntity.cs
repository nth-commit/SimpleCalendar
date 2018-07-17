using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleCalendar.Api.Core.Data
{
    public class RegionRoleEntity
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public RegionPermission Permissions { get; set; } = RegionPermission.None;

        /// <summary>
        /// A permissions bitmask that is applied to the child regions. As permissions are inherited,
        /// these are the permissions that are granted for this role on top of the permissions for this
        /// region.
        /// </summary>
        public RegionPermission ChildPermissions { get; set; } = RegionPermission.None;

        /// <summary>
        /// A permissions bitmask that is applied to the parent region.
        /// </summary>
        public RegionPermission ParentPermissions { get; set; } = RegionPermission.None;
    }

    public static class RegionRoleEntityExtensions
    {
        public static bool HasPermission(this RegionRoleEntity regionRole, RegionPermission permission) =>
            regionRole.AnyPermissions(p => p.HasFlag(permission));

        public static bool IsWriter(this RegionRoleEntity regionRole) =>
            regionRole.AnyPermissions(p => (p & RegionPermission.Writer) != 0);

        private static bool AnyPermissions(this RegionRoleEntity regionRole, Func<RegionPermission, bool> pred) =>
            new RegionPermission[] { regionRole.Permissions, regionRole.ChildPermissions, regionRole.ParentPermissions }
                .Any(pred);
    }
}
