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
        private new const string UserId = "TheCurrentUser";

        public GivenIHaveMembershipsInTheRegionHierarchy() => InitializeAsync().GetAwaiter().GetResult();

        private async Task InitializeAsync()
        {
            await this.GivenIAmARegionAdministratorAsync(UserId, Level3RegionId);
            await this.GivenARegionRoleAsync(UserId, Level2ARegionId, RegionMembershipRole.User);
            await this.GivenARegionRoleAsync(UserId, Level2BRegionId, RegionMembershipRole.User);

            await this.GivenARegionRoleAsync("SomeOtherUser", Level1RegionId, RegionMembershipRole.User);
        }

        [Fact]
        public async Task WhenIGetMyMemberships_ItReturnsAllMyMemberships()
        {
            var memberships = await QueryMyMembershipsAndAssertOKAsync();
            Assert.Equal(3, memberships.Where(m => m.UserId == UserId).Count());
        }

        [Fact]
        public async Task WhenIGetMyMemberships_ItReturnsOnlyMyMemberships()
        {
            var memberships = await QueryMyMembershipsAndAssertOKAsync();
            Assert.Equal(3, memberships.Count());
        }
    }
}
