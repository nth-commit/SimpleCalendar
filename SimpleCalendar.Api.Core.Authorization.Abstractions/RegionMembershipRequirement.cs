using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleCalendar.Api.Core.Authorization
{
    public class RegionMembershipRequirement : IAuthorizationRequirement
    {

        public static QueryRequirement Query(string regionId, string email) => new QueryRequirement()
        {
            RegionId = regionId,
            Email = email
        };

        public class QueryRequirement : RegionMembershipRequirement
        {
            public string RegionId { get; set; }

            public string Email { get; set; }
        }
    }
}
