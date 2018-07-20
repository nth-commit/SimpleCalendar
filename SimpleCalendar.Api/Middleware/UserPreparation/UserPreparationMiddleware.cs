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
            var email = GetEmailFromUserInfoClaims(userInfoClaims);
            var user = await coreDbContext.Users.FindAsync(email);
            if (user == null)
            {
                await AddUserAsync(coreDbContext, userInfoClaims);
            }
            else
            {
                await UpdateUserAsync(coreDbContext, user, userInfoClaims);
            }

            await coreDbContext.SaveChangesAsync();
        }

        private async Task AddUserAsync(CoreDbContext coreDbContext, IEnumerable<Claim> userInfoClaims)
        {
            var sub = GetSubFromUserInfoClaims(userInfoClaims);
            var email = GetEmailFromUserInfoClaims(userInfoClaims);

            await coreDbContext.Users.AddAsync(new UserEntity()
            {
                Email = email,
                ClaimsBySubJson = JsonConvert.SerializeObject(new Dictionary<string, Dictionary<string, string>>()
                    {
                        { sub, userInfoClaims.ToDictionary(c => c.Type, c => c.Value) }
                    }),
                ClaimsBySubVersion = 1,
                OriginatingSub = sub
            });

            await coreDbContext.SaveChangesAsync();
        }

        private async Task UpdateUserAsync(CoreDbContext coreDbContext, UserEntity user, IEnumerable<Claim> userInfoClaims)
        {
            var sub = GetSubFromUserInfoClaims(userInfoClaims);

            bool shouldUpdate = false;

            if (string.IsNullOrEmpty(user.OriginatingSub))
            {
                shouldUpdate = true;
                user.OriginatingSub = sub;
            }

            var claimsBySub = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(user.ClaimsBySubJson);
            var claims = userInfoClaims.ToDictionary(c => c.Type, c => c.Value);
            if (!claimsBySub.TryGetValue(sub, out Dictionary<string, string> existingClaims) ||
                !AreDictionariesEqual(existingClaims, claims))
            {
                // TODO: Test
                shouldUpdate = true;
                claimsBySub[sub] = claims;
                user.ClaimsBySubJson = JsonConvert.SerializeObject(claimsBySub);
            }

            if (shouldUpdate)
            {
                coreDbContext.Users.Update(user);
                await coreDbContext.SaveChangesAsync();
            }
        }

        private string GetEmailFromUserInfoClaims(IEnumerable<Claim> userInfoClaims) =>
            userInfoClaims.First(c => c.Type == "email").Value;

        private string GetSubFromUserInfoClaims(IEnumerable<Claim> userInfoClaims) =>
            userInfoClaims.First(c => c.Type == "sub").Value;

        private bool AreDictionariesEqual(Dictionary<string, string> a, Dictionary<string, string> b)
        {
            if (a.Count() != b.Count())
            {
                return false;
            }

            IOrderedEnumerable<KeyValuePair<string, string>> order(Dictionary<string, string> dict) =>
                dict.OrderBy(kvp => kvp.Key);

            return order(a).SequenceEqual(order(b));
        }
    }
}
