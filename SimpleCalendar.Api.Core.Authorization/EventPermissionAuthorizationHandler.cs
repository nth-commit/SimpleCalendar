using Microsoft.AspNetCore.Authorization;
using SimpleCalendar.Api.Core.Data;
using SimpleCalendar.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCalendar.Api.Core.Authorization
{
    public class EventPermissionAuthorizationHandler : AuthorizationHandler<EventPermissionRequirement, EventEntity>
    {
        private readonly IRegionRolesAccessor _regionRolesAccessor;
        private readonly IEventPermissionResolver _eventPermissionResolver;

        public EventPermissionAuthorizationHandler(
            IRegionRolesAccessor regionRolesAccessor,
            IEventPermissionResolver eventPermissionResolver)
        {
            _regionRolesAccessor = regionRolesAccessor;
            _eventPermissionResolver = eventPermissionResolver;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, EventPermissionRequirement requirement, EventEntity resource)
        {
            if (IsSuccessful(context, requirement, resource))
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }

        private bool IsSuccessful(
            AuthorizationHandlerContext context,
            EventPermissionRequirement requirement,
            EventEntity resource) =>
                _eventPermissionResolver.HasPermission(
                    context.User,
                    resource,
                    requirement.Permission,
                    _regionRolesAccessor.RegionRoles);
    }
}
