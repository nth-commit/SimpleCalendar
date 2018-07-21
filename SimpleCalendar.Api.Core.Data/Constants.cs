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
            public const string SuperAdministrator = Framework.Constants.RegionRoles.SuperAdministrator;
            public const string Administrator = Framework.Constants.RegionRoles.Administrator;
            public const string User = Framework.Constants.RegionRoles.User;
        }
    }
}
