using SimpleCalendar.Api.Core.Data;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCalendar.Api.Core.Authorization
{
    public interface IEventPermissionResolver
    {
        bool HasPermission(
            ClaimsPrincipal user,
            EventEntity ev,
            EventPermissions permission,
            IEnumerable<RegionRoleEntity> regionRoles);
    }
}
