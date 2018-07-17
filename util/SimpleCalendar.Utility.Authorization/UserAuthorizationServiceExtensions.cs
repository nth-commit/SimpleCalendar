using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCalendar.Utility.Authorization
{
    public static class UserAuthorizationServiceExtensions
    {
        public static async Task AssertAuthorizedAsync(
            this IUserAuthorizationService userAuthorizationService,
            object resource,
            IAuthorizationRequirement requirement)
        {
            var result = await userAuthorizationService.AuthorizeAsync(resource, requirement);
            if (!result.Succeeded)
            {
                throw new UserUnauthorizedException();
            }
        }

        public static async Task<bool> IsAuthorizedAsync(
            this IUserAuthorizationService userAuthorizationService,
            object resource,
            IAuthorizationRequirement requirement)
                => (await userAuthorizationService.AuthorizeAsync(resource, new IAuthorizationRequirement[] { requirement })).Succeeded;

        public static async Task<bool> IsAuthorizedAsync(
            this IUserAuthorizationService userAuthorizationService,
            IAuthorizationRequirement requirement)
                => (await userAuthorizationService.AuthorizeAsync(null, new IAuthorizationRequirement[] { requirement })).Succeeded;

        public static Task<AuthorizationResult> AuthorizeAsync(
            this IUserAuthorizationService userAuthorizationService,
            object resource,
            IAuthorizationRequirement requirement)
                => userAuthorizationService.AuthorizeAsync(resource, new IAuthorizationRequirement[] { requirement });
    }
}
