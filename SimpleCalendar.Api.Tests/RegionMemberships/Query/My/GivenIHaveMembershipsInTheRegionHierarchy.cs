using SimpleCalendar.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SimpleCalendar.Api.UnitTests.RegionMemberships.Query.My
{
    public class GivenIHaveMembershipsInTheRegionHierarchy : GivenARegionHierarchy
    {
        private const string UserId = "TheCurrentUser";

        public GivenIHaveMembershipsInTheRegionHierarchy() => InitializeAsync().GetAwaiter().GetResult();

        private async Task InitializeAsync()
        {
            await this.GivenIAmARegionSuperAdministratorAsync(UserId, Level3RegionId);
            await this.GivenARegionMembershipAsync(UserId, Level2ARegionId, Core.Data.Constants.RegionRoles.User);
            await this.GivenARegionMembershipAsync(UserId, Level2BRegionId, Core.Data.Constants.RegionRoles.User);

            await this.GivenARegionMembershipAsync("SomeOtherUser", Level1RegionId, Core.Data.Constants.RegionRoles.User);
        }

        [Fact]
        public async Task WhenIGetMyMemberships_ItReturnsAllMyMemberships()
        {
            var memberships = await QueryMyMembershipsAndAssertOKAsync();
            Assert.Equal(3, memberships.Where(m => m.UserEmail == UserId).Count());
        }

        [Fact]
        public async Task WhenIGetMyMemberships_ItReturnsOnlyMyMemberships()
        {
            var memberships = await QueryMyMembershipsAndAssertOKAsync();
            Assert.Equal(3, memberships.Count());
        }
    }
}
