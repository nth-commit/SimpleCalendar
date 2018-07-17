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
            EventPermissions permission,
            EventEntity ev,
            ClaimsPrincipal user,
            Lazy<Task<IEnumerable<RegionRoleEntity>>> lazyRegionRolesTask,
            Lazy<Task<IEnumerable<RegionMembershipEntity>>> lazyRegionMembershipsTask)
        {
            switch (permission)
            {
                case EventPermissions.View:
                    return await CanViewEventAsync(ev, user, lazyRegionRolesTask, lazyRegionMembershipsTask);
                default:
                    return false;
            }
        }

        private async Task<bool> CanViewEventAsync(
            EventEntity ev,
            ClaimsPrincipal user,
            Lazy<Task<IEnumerable<RegionRoleEntity>>> lazyRegionRolesTask,
            Lazy<Task<IEnumerable<RegionMembershipEntity>>> lazyRegionMembershipsTask)
        {
            if (ev.IsPublished)
            {
                return (
                    ev.IsPublic ||
                    await HasRegionMembershipAsync(ev.Region, user, lazyRegionRolesTask, lazyRegionMembershipsTask));
            }

            return (
                IsCreator(ev, user) ||
                await CanViewDraftEventsAsync(ev.Region, user, lazyRegionRolesTask, lazyRegionMembershipsTask));
        }

        private bool IsCreator(EventEntity ev, ClaimsPrincipal user) =>
            !string.IsNullOrEmpty(ev.CreatedByEmail) && ev.CreatedByEmail == user.GetUserEmail();

        private Task<bool> HasRegionMembershipAsync(
            RegionEntity region,
            ClaimsPrincipal user,
            Lazy<Task<IEnumerable<RegionRoleEntity>>> lazyRegionRolesTask,
            Lazy<Task<IEnumerable<RegionMembershipEntity>>> lazyRegionMembershipsTask) =>
                HasRegionPermissionAsync(RegionPermission.None, user, region, lazyRegionRolesTask, lazyRegionMembershipsTask);

        private Task<bool> CanViewDraftEventsAsync(
            RegionEntity region,
            ClaimsPrincipal user,
            Lazy<Task<IEnumerable<RegionRoleEntity>>> lazyRegionRolesTask,
            Lazy<Task<IEnumerable<RegionMembershipEntity>>> lazyRegionMembershipsTask) =>
                HasRegionPermissionAsync(RegionPermission.Events_Write, user, region, lazyRegionRolesTask, lazyRegionMembershipsTask);

        private async Task<bool> HasRegionPermissionAsync(
            RegionPermission regionPermission,
            ClaimsPrincipal user,
            RegionEntity region,
            Lazy<Task<IEnumerable<RegionRoleEntity>>> lazyRegionRolesTask,
            Lazy<Task<IEnumerable<RegionMembershipEntity>>> lazyRegionMembershipsTask)
        {
            if (!user.Identity.IsAuthenticated)
            {
                return false;
            }

            var regionRolesTask = lazyRegionRolesTask.Value;
            var regionMembershipsTask = lazyRegionMembershipsTask.Value;
            await Task.WhenAll(regionRolesTask, regionMembershipsTask);
            return _regionPermissionResolver.HasPermission(
                regionPermission,
                region,
                regionRolesTask.Result,
                regionMembershipsTask.Result);
        }
    }
}
