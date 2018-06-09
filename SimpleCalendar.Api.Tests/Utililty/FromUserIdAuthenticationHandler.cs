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
    public class FromUserIdAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly IUserIdContainer _userIdContainer;

        public FromUserIdAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IUserIdContainer userIdContainer)
            : base(options, logger, encoder, clock)
        {
            _userIdContainer = userIdContainer;
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            AuthenticateResult result = null;

            var userId = _userIdContainer.Value;
            if (string.IsNullOrEmpty(userId))
            {
                result = AuthenticateResult.Fail("No user ID was set");
            }
            else
            {
                var identity = new ClaimsIdentity("TestAuthenticationType");
                identity.AddClaim(new Claim("sub", userId));
                var principal = new ClaimsPrincipal(identity);
                result = AuthenticateResult.Success(new AuthenticationTicket(principal, "TestScheme"));
            }

            return Task.FromResult(result);
        }
    }
}
