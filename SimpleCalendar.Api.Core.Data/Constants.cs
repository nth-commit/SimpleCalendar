using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCalendar.Api.Core.Data
{
    public static class Constants
    {
        public const string RootRegionId = "ROOT";

        public static class RegionRoles
        {
            public const string SuperAdministrator = "ROLE_SUPER_ADMINISTRATOR";
            public const string Administrator = "ROLE_ADMINISTRATOR";
            public const string User = "ROLE_USER";
        }
    }
}
