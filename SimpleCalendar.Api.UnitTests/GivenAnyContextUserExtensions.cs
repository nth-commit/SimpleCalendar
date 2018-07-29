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
        public static void GivenIAmARootSuperAdministrator(this GivenAnyContext context)
            => context.GivenIHaveAnEmail("michaelfry2002@gmail.com");

        public static Task GivenIAmARegionUserAsync(this GivenAnyContext context, string email, string regionId)
            => context.GivenIHaveARoleInARegion(email, regionId, Core.Data.Constants.RegionRoles.User);

        public static Task GivenIAmARegionAdministratorAsync(this GivenAnyContext context, string email, string regionId)
            => context.GivenIHaveARoleInARegion(email, regionId, Core.Data.Constants.RegionRoles.Administrator);

        public static Task GivenIAmARegionSuperAdministratorAsync(this GivenAnyContext context, string email, string regionId)
            => context.GivenIHaveARoleInARegion(email, regionId, Core.Data.Constants.RegionRoles.SuperAdministrator);

        public static void GivenIHaveAnEmail(this GivenAnyContext context, string email)
            => context.UserEmail.Setup(x => x.Value).Returns(email);

        private static async Task GivenIHaveARoleInARegion(
            this GivenAnyContext context,
            string email,
            string regionId,
            string regionRoleId)
        {
            await context.GivenARegionMembershipAsync(email, regionId, regionRoleId);
            context.GivenIHaveAnEmail(email);
        }
    }
}
