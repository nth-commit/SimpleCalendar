using IdentityModel.Client;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SimpleCalendar.Api.Middleware.UserPreparation.Impl
{
    public class UserInfoService : IUserInfoService
    {
        public async Task<IEnumerable<Claim>> GetUserInfoAsync(HttpContext httpContext)
        {
            // TODO: Cache

            var authority = httpContext.User.Claims.Where(c => c.Type == "iss").First();
            var discoveryClient = new DiscoveryClient(authority.Value);
            var discoveryResponse = await discoveryClient.GetAsync();

            var userInfoClient = new UserInfoClient(discoveryResponse.UserInfoEndpoint);
            var userInfo = await userInfoClient.GetAsync(httpContext.GetSecurityToken());

            return userInfo.Claims;
        }
    }
}
