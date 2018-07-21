﻿using Microsoft.AspNetCore.Authorization;
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
        private readonly IRegionRoleCache _regionRoleCache;
        private readonly IEventPermissionResolver _eventPermissionResolver;

        public EventPermissionAuthorizationHandler(
            IRegionRoleCache regionRoleCache,
            IEventPermissionResolver eventPermissionResolver)
        {
            _regionRoleCache = regionRoleCache;
            _eventPermissionResolver = eventPermissionResolver;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, EventPermissionRequirement requirement, EventEntity resource)
        {
            if (await IsSuccessfulAsync(context, requirement, resource))
            {
                context.Succeed(requirement);
            }
        }

        private Task<bool> IsSuccessfulAsync(AuthorizationHandlerContext context, EventPermissionRequirement requirement, EventEntity resource) =>
            _eventPermissionResolver.HasPermissionAsync(
                context.User,
                resource,
                requirement.Permission,
                new Lazy<Task<IEnumerable<RegionRoleEntity>>>(
                    () => _regionRoleCache.ListAsync(),
                    isThreadSafe: true));
    }
}