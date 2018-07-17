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
            ClaimsPrincipal user,
            RegionEntity region,
            RegionPermission permission,
            IEnumerable<RegionRoleEntity> regionRoles);
    }
}
