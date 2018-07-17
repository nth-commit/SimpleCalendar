using Microsoft.AspNetCore.Authorization;
using SimpleCalendar.Api.Core.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleCalendar.Api.Core.Authorization
{
    public class RegionPermissionRequirement : IAuthorizationRequirement
    {
        public RegionPermission Permission { get; private set; }

        public static RegionPermissionRequirement CreateEvents => CreateRequirement(RegionPermission.Events_WriteDraft);
        public static RegionPermissionRequirement PublishEvents => CreateRequirement(RegionPermission.Events_Write);

        public static RegionPermissionRequirement ViewMemberships => CreateRequirement(RegionPermission.Memberships_Read);

        public static RegionPermissionRequirement CreateWriterMemberships => CreateRequirement(RegionPermission.Memberships_WriteWriter);
        public static RegionPermissionRequirement CreateReaderMemberships => CreateRequirement(RegionPermission.Memberships_WriteReader);

        private static RegionPermissionRequirement CreateRequirement(RegionPermission permission) =>
            new RegionPermissionRequirement() { Permission = permission };
    }
}
