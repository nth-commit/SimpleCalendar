using Microsoft.AspNetCore.Authorization;
using SimpleCalendar.Framework.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCalendar.Utility.Authorization
{
    public class DefaultUserAuthorizationService : IUserAuthorizationService
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly IUserAccessor _claimsPrincipalAccessor;

        public DefaultUserAuthorizationService(
            IAuthorizationService authorizationService,
            IUserAccessor claimsPrincipalAccessor)
        {
            _authorizationService = authorizationService;
            _claimsPrincipalAccessor = claimsPrincipalAccessor;
        }

        public Task<AuthorizationResult> AuthorizeAsync(object resource, IEnumerable<IAuthorizationRequirement> requirements)
            => _authorizationService.AuthorizeAsync(_claimsPrincipalAccessor.User, resource, requirements);

        public Task<AuthorizationResult> AuthorizeAsync(object resource, string policyName)
            => _authorizationService.AuthorizeAsync(_claimsPrincipalAccessor.User, resource, policyName);
    }
}
