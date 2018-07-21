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

        public bool HasPermission(
            ClaimsPrincipal user,
            EventEntity ev,
            EventPermissions permission,
            IEnumerable<RegionRoleEntity> regionRoles)
        {
            switch (permission)
            {
                case EventPermissions.View:
                    return CanViewEvent(user, ev, regionRoles);
                default:
                    return false;
            }
        }

        private bool CanViewEvent(
            ClaimsPrincipal user,
            EventEntity ev,
            IEnumerable<RegionRoleEntity> regionRoles)
        {
            if (ev.IsPublished)
            {
                return (
                    ev.IsPublic ||
                    HasRegionMembership(ev.Region, user, regionRoles));
            }

            return (
                IsCreator(ev, user) ||
                CanViewDraftEvents(ev.Region, user, regionRoles));
        }

        private bool IsCreator(EventEntity ev, ClaimsPrincipal user) =>
            !string.IsNullOrEmpty(ev.CreatedByEmail) && ev.CreatedByEmail == user.GetUserEmail();

        private bool HasRegionMembership(
            RegionEntity region,
            ClaimsPrincipal user,
            IEnumerable<RegionRoleEntity> regionRoles) =>
                HasRegionPermission(user, region, RegionPermission.None, regionRoles);

        private bool CanViewDraftEvents(
            RegionEntity region,
            ClaimsPrincipal user,
            IEnumerable<RegionRoleEntity> regionRoles) =>
                HasRegionPermission(user, region, RegionPermission.Events_Write, regionRoles);

        private bool HasRegionPermission(
            ClaimsPrincipal user,
            RegionEntity region,
            RegionPermission regionPermission,
            IEnumerable<RegionRoleEntity> regionRoles)
        {
            if (!user.Identity.IsAuthenticated)
            {
                return false;
            }

            return _regionPermissionResolver.HasPermission(
                user,
                region,
                regionPermission,
                regionRoles);
        }
    }
}
