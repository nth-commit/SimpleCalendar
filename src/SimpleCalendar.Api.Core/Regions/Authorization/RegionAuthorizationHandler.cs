using Microsoft.AspNetCore.Authorization;
using SimpleCalendar.Api.Core.Data;
using SimpleCalendar.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCalendar.Api.Core.Regions.Authorization
{
    public class RegionAuthorizationHandler : AuthorizationHandler<RegionRequirement, RegionEntity>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RegionRequirement requirement, RegionEntity resource)
        {
            if (requirement is CreateEventsRequirement)
            {
                if (resource.IsAdministrator(context.User.GetUserId()))
                {
                    context.Succeed(requirement);
                }
            }
            else if (requirement is PublishEventsRequirement)
            {
                if (resource.IsAdministrator(context.User.GetUserId()))
                {
                    context.Succeed(requirement);
                }
            }

            return Task.CompletedTask;
        }
    }
}
