using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCalendar.Framework
{
    public static class ClaimsExtensions
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

        public static void AddRegionMembershipRole(
            this ClaimsIdentity claimsIdentity,
            RegionMembershipRoleClaimValue claimValue) =>
                claimsIdentity.AddClaim(new Claim(
                    ClaimsIdentity.DefaultRoleClaimType,
                    $"{claimValue.RegionId}:{claimValue.RegionRoleId}"));

        public static IEnumerable<RegionMembershipRoleClaimValue> GetRegionMembershipRoles(this ClaimsPrincipal principal) =>
            principal.Claims
                .Where(c => c.Type == ClaimsIdentity.DefaultRoleClaimType)
                .Select(c => c.Value.Split(':'))
                .Select(c => new RegionMembershipRoleClaimValue()
                {
                    RegionId = c[0],
                    RegionRoleId = c[1]
                });

        public class RegionMembershipRoleClaimValue
        {
            public string RegionId { get; set; }

            public string RegionRoleId { get; set; }
        }
    }
}
