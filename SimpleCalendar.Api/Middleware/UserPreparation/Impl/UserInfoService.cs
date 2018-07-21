using IdentityModel.Client;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SimpleCalendar.Api.Middleware.UserPreparation.Impl
{
    public class UserInfoService : IUserInfoService
    {
        private readonly IMemoryCache _memoryCache;

        public UserInfoService(
            IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public async Task<IEnumerable<Claim>> GetUserInfoAsync(HttpContext httpContext)
        {
            var securityToken = httpContext.GetSecurityToken();
            return await _memoryCache.GetOrCreateAsync(securityToken, async (entry) =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);

                var authority = httpContext.User.Claims.Where(c => c.Type == "iss").First();
                var discoveryClient = new DiscoveryClient(authority.Value);
                var discoveryResponse = await discoveryClient.GetAsync();

                var userInfoClient = new UserInfoClient(discoveryResponse.UserInfoEndpoint);
                var userInfo = await userInfoClient.GetAsync(httpContext.GetSecurityToken());

                return userInfo.Claims;
            });
        }
    }
}
