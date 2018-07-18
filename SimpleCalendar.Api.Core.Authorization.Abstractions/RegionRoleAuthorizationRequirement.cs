using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleCalendar.Api.Core.Authorization
{
    public class RegionRoleAuthorizationRequirement : IAuthorizationRequirement
    {
        public static QueryRegionRoles Query() => new QueryRegionRoles();

        public class QueryRegionRoles : RegionRoleAuthorizationRequirement { }
    }
}
