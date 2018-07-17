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
        }

        public static string GetUserEmail(
            this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal.Claims.Where(c => c.Type == "email").Select(c => c.Value).FirstOrDefault();
        }
    }
}
