using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCalendar.Utility.Authorization
{
    public interface IUserAuthorizationService
    {
        Task<AuthorizationResult> AuthorizeAsync(object resource, IEnumerable<IAuthorizationRequirement> requirements);
        Task<AuthorizationResult> AuthorizeAsync(object resource, string policyName);
    }
}
