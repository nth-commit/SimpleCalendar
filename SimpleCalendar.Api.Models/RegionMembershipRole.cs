using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCalendar.Api.Models
{
    [Flags]
    public enum RegionMembershipRole
    {
        Unknown = 0,

        User = 1,

        Administrator = 2
    }
}
