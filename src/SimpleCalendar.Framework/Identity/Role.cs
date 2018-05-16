using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCalendar.Framework.Identity
{
    [Flags]
    public enum Role
    {
        Unknown = 0,

        User = 1,

        Administrator = 2
    }
}
