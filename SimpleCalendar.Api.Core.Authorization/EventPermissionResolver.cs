using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using SimpleCalendar.Api.Core.Data;
using SimpleCalendar.Framework;

namespace SimpleCalendar.Api.Core.Authorization
{
    public class EventPermissionResolver : IEventPermissionResolver
    {
        private readonly IRegionPermissionResolver _regionPermissionResolver;

        public EventPermissionResolver(
            IRegionPermissionResolver regionPermissionResolver)
        {
            _regionPermissionResolver = regionPermissionResolver;
        }

        public async Task<bool> HasPermissionAsync(
            ClaimsPrincipal user,
            EventEntity ev,
            EventPermissions permission,
            Lazy<Task<IEnumerable<RegionRoleEntity>>> lazyRegionRolesTask)
        {
            switch (permission)
            {
                case EventPermissions.View:
                    return await CanViewEventAsync(user, ev, lazyRegionRolesTask);
                default:
                    return false;
            }
        }

        private async Task<bool> CanViewEventAsync(
            ClaimsPrincipal user,
            EventEntity ev,
            Lazy<Task<IEnumerable<RegionRoleEntity>>> lazyRegionRolesTask)
        {
            if (ev.IsPublished)
            {
                return (
                    ev.IsPublic ||
                    await HasRegionMembershipAsync(ev.Region, user, lazyRegionRolesTask));
            }

            return (
                IsCreator(ev, user) ||
                await CanViewDraftEventsAsync(ev.Region, user, lazyRegionRolesTask));
        }

        private bool IsCreator(EventEntity ev, ClaimsPrincipal user) =>
            !string.IsNullOrEmpty(ev.CreatedByEmail) && ev.CreatedByEmail == user.GetUserEmail();

        private Task<bool> HasRegionMembershipAsync(
            RegionEntity region,
            ClaimsPrincipal user,
            Lazy<Task<IEnumerable<RegionRoleEntity>>> lazyRegionRolesTask) =>
                HasRegionPermissionAsync(user, region, RegionPermission.None, lazyRegionRolesTask);

        private Task<bool> CanViewDraftEventsAsync(
            RegionEntity region,
            ClaimsPrincipal user,
            Lazy<Task<IEnumerable<RegionRoleEntity>>> lazyRegionRolesTask) =>
                HasRegionPermissionAsync(user, region, RegionPermission.Events_Write, lazyRegionRolesTask);

        private async Task<bool> HasRegionPermissionAsync(
            ClaimsPrincipal user,
            RegionEntity region,
            RegionPermission regionPermission,
            Lazy<Task<IEnumerable<RegionRoleEntity>>> lazyRegionRolesTask)
        {
            if (!user.Identity.IsAuthenticated)
            {
                return false;
            }

            return _regionPermissionResolver.HasPermission(
                user,
                region,
                regionPermission,
                await lazyRegionRolesTask.Value);
        }
    }
}
