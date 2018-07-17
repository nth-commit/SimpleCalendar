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
        Task<bool> HasPermissionAsync(
            ClaimsPrincipal user,
            EventEntity ev,
            EventPermissions permission,
            Lazy<Task<IEnumerable<RegionRoleEntity>>> lazyRegionRolesTask);
    }
}
