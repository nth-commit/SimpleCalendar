using Microsoft.AspNetCore.Authorization;
using SimpleCalendar.Api.Core.Data;
using SimpleCalendar.Framework.Identity;
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
                if (IsRegionAdministrator(context.User, resource))
                {
                    context.Succeed(requirement);
                }
            }
            else if (requirement is PublishEventsRequirement)
            {
                if (IsRegionAdministrator(context.User, resource))
                {
                    context.Succeed(requirement);
                }
            }

            return Task.CompletedTask;
        }

        private bool IsRegionAdministrator(ClaimsPrincipal user, RegionEntity region)
        {
            if (region.Roles == null)
            {
                throw new InvalidOperationException("Roles cannot be null to determine if user is an adinistrator");
            }

            return region.Roles.Where(r => r.UserId == user.Identity.Name).Any(r => r.Role.HasFlag(Role.Administrator)) ||
                (region.Id != Data.Constants.RootRegionId && IsRegionAdministrator(user, region.Parent));
        }

        private RegionEntity GetRootRegion(RegionEntity region)
        {
            if (region.Id == Data.Constants.RootRegionId)
            {
                return region;
            }

            if (region.Parent == null)
            {
                throw new InvalidOperationException("Could not resolve root region");
            }

            return GetRootRegion(region.Parent);
        }
    }
}
