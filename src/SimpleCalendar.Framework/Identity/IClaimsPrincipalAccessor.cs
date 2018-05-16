using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCalendar.Framework.Identity
{
    public interface IClaimsPrincipalAccessor
    {
        ClaimsPrincipal ClaimsPrincipal { get; }
    }
}
