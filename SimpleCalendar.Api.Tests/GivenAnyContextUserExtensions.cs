using SimpleCalendar.Api.Core.Data;
using SimpleCalendar.Api.Models;
using SimpleCalendar.Framework.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCalendar.Api.UnitTests
{
    public static class GivenAnyContextUserExtensions
    {
        public static void GivenIAmARootAdministrator(this GivenAnyContext context)
            => context.GivenIHaveAUserId("google-oauth2|103074202427969604113");

        public static Task GivenIAmARegionUserAsync(this GivenAnyContext context, string userId, string regionId)
            => context.GivenIHaveARoleInARegion(userId, regionId, RegionMembershipRole.User);

        public static Task GivenIAmARegionAdministratorAsync(this GivenAnyContext context, string userId, string regionId)
            => context.GivenIHaveARoleInARegion(userId, regionId, RegionMembershipRole.Administrator);

        public static void GivenIHaveAUserId(this GivenAnyContext context, string userId)
            => context.UserId.Setup(x => x.Value).Returns(userId);

        private static async Task GivenIHaveARoleInARegion(
            this GivenAnyContext context,
            string userId,
            string regionId,
            RegionMembershipRole role)
        {
            await context.GivenARegionRoleAsync(userId, regionId, role);
            context.GivenIHaveAUserId(userId);
        }
    }
}
