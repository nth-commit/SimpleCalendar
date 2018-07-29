using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace SimpleCalendar.Api.UnitTests.Utililty
{
    public class UserEmailAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly IUserEmailContainer _userEmailContainer;

        public UserEmailAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IUserEmailContainer userEmailContainer)
            : base(options, logger, encoder, clock)
        {
            _userEmailContainer = userEmailContainer;
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            AuthenticateResult result = null;

            var userEmail = _userEmailContainer.Value;
            if (string.IsNullOrEmpty(userEmail))
            {
                result = AuthenticateResult.Fail("No user ID was set");
            }
            else
            {
                var identity = new ClaimsIdentity("TestAuthenticationType");
                identity.AddClaim(new Claim("sub", "test|123"));
                identity.AddClaim(new Claim("email", userEmail));
                var principal = new ClaimsPrincipal(identity);
                result = AuthenticateResult.Success(new AuthenticationTicket(principal, "TestScheme"));
            }

            return Task.FromResult(result);
        }
    }
}
