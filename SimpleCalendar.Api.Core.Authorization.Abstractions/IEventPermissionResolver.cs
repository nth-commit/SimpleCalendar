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
            EventPermissions permission,
            EventEntity ev,
            ClaimsPrincipal user,
            Lazy<Task<IEnumerable<RegionRoleEntity>>> lazyRegionRolesTask,
            Lazy<Task<IEnumerable<RegionMembershipEntity>>> lazyRegionMembershipsTask);
    }
}
