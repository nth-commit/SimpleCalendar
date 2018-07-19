using IdentityModel.Client;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SimpleCalendar.Api.Core.Data;
using SimpleCalendar.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SimpleCalendar.Api.Middleware.UserPreparation
{
    public class UserPreparationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IUserInfoService _userInfoService;

        public UserPreparationMiddleware(
            RequestDelegate next,
            IUserInfoService userInfoService)
        {
            _next = next;
            _userInfoService = userInfoService;
        }

        public async Task InvokeAsync(
            HttpContext context,
            CoreDbContext coreDbContext)
        {
            if (context.User.Identity.IsAuthenticated)
            {
                var userInfoClaims = await _userInfoService.GetUserInfoAsync(context);

                var regionMembershipRoleClaims = await GetRegionMembershipRoleClaimsAsync(userInfoClaims, coreDbContext);
                if (!regionMembershipRoleClaims.Any())
                {
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    return;
                }

                await EnsureUserAsync(coreDbContext, userInfoClaims);

                context.User = new ClaimsPrincipal(new ClaimsIdentity(
                    regionMembershipRoleClaims.Concat(userInfoClaims),
                    context.User.Identity.AuthenticationType));
            }

            await _next(context);
        }

        private async Task<IEnumerable<Claim>> GetRegionMembershipRoleClaimsAsync(IEnumerable<Claim> userInfoClaims, CoreDbContext coreDbContext)
        {
            var userEmail = userInfoClaims.First(c => c.Type == "email").Value;
            var regionMemberships = await coreDbContext.GetRegionMembershipsAsync(userEmail);
            return regionMemberships
                .Select(rm => new ClaimsExtensions.RegionMembershipRoleClaimValue()
                {
                    RegionId = rm.RegionId,
                    RegionRoleId = rm.RegionRoleId
                })
                .Select(rm => rm.ToClaim());
        }

        private async Task EnsureUserAsync(CoreDbContext coreDbContext, IEnumerable<Claim> userInfoClaims)
        {
            // TODO: Cache / ETag
            var userEmail = userInfoClaims.First(c => c.Type == "email").Value;
            var sub = userInfoClaims.First(c => c.Type == "sub").Value;

            var user = await coreDbContext.Users.FindAsync(userEmail);
            if (user == null)
            {
                await coreDbContext.Users.AddAsync(new UserEntity()
                {
                    Email = userEmail,
                    ClaimsBySubJson = JsonConvert.SerializeObject(new Dictionary<string, Dictionary<string, string>>()
                    {
                        { sub, userInfoClaims.ToDictionary(c => c.Type, c => c.Value) }
                    }),
                    ClaimsBySubVersion = 1,
                    OriginatingSub = sub
                });
            }
            else
            {
                var claimsBySub = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(user.ClaimsBySubJson);
                claimsBySub[sub] = userInfoClaims.ToDictionary(c => c.Type, c => c.Value);
                user.ClaimsBySubJson = JsonConvert.SerializeObject(claimsBySub);

                if (string.IsNullOrEmpty(user.OriginatingSub))
                {
                    user.OriginatingSub = sub;
                }

                coreDbContext.Users.Update(user);
            }

            await coreDbContext.SaveChangesAsync();
        }
    }
}
