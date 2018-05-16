using Microsoft.AspNetCore.Authorization;
using SimpleCalendar.Framework.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCalendar.Utility.Authorization
{
    public class DefaultClaimsPrincipalAuthorizationService : IClaimsPrincipalAuthorizationService
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly IClaimsPrincipalAccessor _claimsPrincipalAccessor;

        public DefaultClaimsPrincipalAuthorizationService(
            IAuthorizationService authorizationService,
            IClaimsPrincipalAccessor claimsPrincipalAccessor)
        {
            _authorizationService = authorizationService;
            _claimsPrincipalAccessor = claimsPrincipalAccessor;
        }

        public Task<AuthorizationResult> AuthorizeAsync(object resource, IEnumerable<IAuthorizationRequirement> requirements)
            => _authorizationService.AuthorizeAsync(_claimsPrincipalAccessor.ClaimsPrincipal, resource, requirements);

        public Task<AuthorizationResult> AuthorizeAsync(object resource, string policyName)
            => _authorizationService.AuthorizeAsync(_claimsPrincipalAccessor.ClaimsPrincipal, resource, policyName);
    }
}
