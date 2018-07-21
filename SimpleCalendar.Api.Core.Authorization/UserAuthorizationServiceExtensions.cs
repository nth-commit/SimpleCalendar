using SimpleCalendar.Api.Core.Authorization;
using SimpleCalendar.Api.Core.Data;
using SimpleCalendar.Utility.Authorization;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCalendar.Utility.Authorization
{
    public static class UserAuthorizationServiceExtensions
    {
        public static Task<bool> CanCreateEventsAsync(this IUserAuthorizationService userAuthorizationService, RegionEntity regionEntity)
            => userAuthorizationService.IsAuthorizedAsync(regionEntity, RegionPermissionRequirement.CreateEvents);

        public static Task<bool> CanPublishEventsAsync(this IUserAuthorizationService userAuthorizationService, RegionEntity regionEntity)
            => userAuthorizationService.IsAuthorizedAsync(regionEntity, RegionPermissionRequirement.PublishEvents);

        public static Task<bool> CanViewEventAsync(this IUserAuthorizationService userAuthorizationService, EventEntity eventEntity)
            => userAuthorizationService.IsAuthorizedAsync(eventEntity, EventPermissionRequirement.View);

        public static Task<bool> CanQueryMembershipsAsync(this IUserAuthorizationService userAuthorizationService, RegionEntity regionEntity)
            => userAuthorizationService.IsAuthorizedAsync(regionEntity, RegionOperationRequirement.QueryMemberships);

        public static Task<bool> CanCreateMembershipAsync(this IUserAuthorizationService userAuthorizationService, RegionEntity regionEntity, string regionRoleId)
            => userAuthorizationService.IsAuthorizedAsync(regionEntity, RegionOperationRequirement.CreateMembership(regionRoleId));

        public static Task<bool> CanDeleteMembershipsAsync(this IUserAuthorizationService userAuthorizationService, RegionEntity regionEntity)
            => userAuthorizationService.IsAuthorizedAsync(regionEntity, RegionOperationRequirement.DeleteMemberships);

        public static Task<bool> CanQueryRolesAsync(this IUserAuthorizationService userAuthorizationService)
            => userAuthorizationService.IsAuthorizedAsync(RegionRoleAuthorizationRequirement.Query());
    }
}
