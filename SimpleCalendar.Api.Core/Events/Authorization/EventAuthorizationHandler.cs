﻿using Microsoft.AspNetCore.Authorization;
using SimpleCalendar.Api.Core.Data;
using SimpleCalendar.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCalendar.Api.Core.Events.Authorization
{
    public class EventAuthorizationHandler : AuthorizationHandler<EventRequirement, EventEntity>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, EventRequirement requirement, EventEntity resource)
        {
            var userId = context.User.GetUserId();
            if (requirement is ViewEventRequirement)
            {
                if (IsAdministrator(context, resource) || IsCreator(context, resource) ||
                    (resource.IsPublished && (resource.Region.IsUser(userId) || resource.IsPublic)))
                {
                    context.Succeed(requirement);
                }
            }

            return Task.CompletedTask;
        }

        private bool IsAdministrator(AuthorizationHandlerContext context, EventEntity resource)
        {
            return resource.Region.IsAdministrator(context.User.GetUserId());
        }

        private bool IsCreator(AuthorizationHandlerContext context, EventEntity resource)
        {
            return !string.IsNullOrEmpty(resource.CreatedById) && resource.CreatedById == context.User.GetUserId();
        }
    }
}
