using SimpleCalendar.Api.Core.Data;
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
            => context.GivenIHaveARoleInARegion(userId, regionId, Role.User);

        public static Task GivenIAmARegionAdministratorAsync(this GivenAnyContext context, string userId, string regionId)
            => context.GivenIHaveARoleInARegion(userId, regionId, Role.Administrator);

        public static void GivenIHaveAUserId(this GivenAnyContext context, string userId)
            => context.UserId.Setup(x => x.Value).Returns(userId);

        private static async Task GivenIHaveARoleInARegion(
            this GivenAnyContext context,
            string userId,
            string regionId,
            Role role)
        {
            var coreDbContext = context.GetCoreDbContext();

            var region = await coreDbContext.GetRegionByCodesAsync(regionId);
            if (region == null)
            {
                throw new Exception("Region does not exist");
            }

            await coreDbContext.RegionRoles.AddAsync(new RegionRoleEntity()
            {
                UserId = userId,
                RegionId = region.Id,
                Role = role
            });
            await coreDbContext.SaveChangesAsync();

            context.GivenIHaveAUserId(userId);
        }
    }
}
