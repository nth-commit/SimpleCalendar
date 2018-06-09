using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCalendar.Framework
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetUserId(
            this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal.Claims.Where(c => c.Type == "sub").Select(c => c.Value).FirstOrDefault();
            // This is where Auth0 stored the sub, needs to be mapped to something like the above for consistency, or investigated what is normal.
            // return claimsPrincipal.Claims.Where(c => c.Properties.Any(p => p.Value == "sub")).Select(c => c.Value).FirstOrDefault();
        }
    }
}
