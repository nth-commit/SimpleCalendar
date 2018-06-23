using Microsoft.AspNetCore.Authorization;
using SimpleCalendar.Api.Core.Data;
using SimpleCalendar.Api.Models;
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
                IfRegionAdministratorThenSucceed(context, requirement, resource);
            }
            else if (requirement is PublishEventsRequirement)
            {
                IfRegionAdministratorThenSucceed(context, requirement, resource);
            }
            else if (requirement is CreateMembershipRequirement createMembershipRequirement)
            {
                if (createMembershipRequirement.Role.HasFlag(RegionMembershipRole.Administrator))
                {
                    IfRegionAdministratorThenSucceed(context, requirement, resource.Parent);
                }
                else
                {
                    IfRegionAdministratorThenSucceed(context, requirement, resource);
                }
            }

            return Task.CompletedTask;
        }

        private void IfRegionAdministratorThenSucceed(AuthorizationHandlerContext context, RegionRequirement requirement, RegionEntity resource)
        {
            if (resource.IsAdministrator(context.User.GetUserId()))
            {
                context.Succeed(requirement);
            }
        }
    }
}
