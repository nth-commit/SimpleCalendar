using Microsoft.AspNetCore.Authorization;
using SimpleCalendar.Api.Core.Data;
using SimpleCalendar.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCalendar.Api.Core.Authorization
{
    public class RegionRoleAuthorizationHandler : AuthorizationHandler<RegionRoleAuthorizationRequirement>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            RegionRoleAuthorizationRequirement requirement)
        {
            if (requirement is RegionRoleAuthorizationRequirement.QueryRegionRoles)
            {
                if (IsAdministratorOrSuperAdministrator(context.User))
                {
                    context.Succeed(requirement);
                }
            }

            return Task.CompletedTask;
        }

        private bool IsAdministratorOrSuperAdministrator(ClaimsPrincipal user)
        {
            return user.GetRegionMembershipRoles().Any(rm =>
                rm.RegionRoleId == Constants.RegionRoles.Administrator ||
                rm.RegionRoleId == Constants.RegionRoles.SuperAdministrator);
        }
    }
}
