using Microsoft.AspNetCore.Authorization;
using SimpleCalendar.Api.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleCalendar.Api.Core.Regions.Authorization
{
    public class RegionRequirement : IAuthorizationRequirement
    {
        public static RegionRequirement CreateMembership(RegionMembershipRole role)
            => new CreateMembershipRequirement()
            {
                Role = role
            };

        public static RegionRequirement DeleteMembership(RegionMembershipRole role)
            => new DeleteMembershipRequirement()
            {
                Role = role
            };
    }
}
