using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SimpleCalendar.Api.Services
{
    public class ClaimsTransformation : IClaimsTransformation
    {
        public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            var googleId = principal.Claims.Where(c => c.Properties.Any(p => p.Value == "sub")).Select(c => c.Value).FirstOrDefault();
            if (string.IsNullOrEmpty(googleId))
            {
                throw new Exception("Invalid user");
            }

            var identity = new ClaimsIdentity(principal.Identity);
            identity.AddClaim(new Claim("sub", googleId));

            return Task.FromResult(new ClaimsPrincipal(identity));
        }
    }
}
