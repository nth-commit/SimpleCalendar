using SimpleCalendar.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SimpleCalendar.Api.UnitTests.RegionMemberships.Query
{
    public class GivenThereAreMembershipsForARegion : GivenARegionHierarchy
    {
        protected const string RegionId = Level1RegionId;
        protected const int RegionUserCount = 3;
        protected const int RegionAdministratorCount = 3;

        public GivenThereAreMembershipsForARegion() => InitializeAsync().GetAwaiter().GetResult();

        private async Task InitializeAsync()
        {
            this.GivenIAmARootAdministrator();

            for (var id = 0; id < RegionUserCount; id++)
            {
                await CreateMembershipAsync(id, RegionMembershipRole.User);
            }

            for (var id = 0; id < RegionUserCount; id++)
            {
                await CreateMembershipAsync(id, RegionMembershipRole.Administrator);
            }

            await this.GivenARegionRoleAsync("SomeOtherUser", Level3RegionId, RegionMembershipRole.User);
        }

        private async Task CreateMembershipAsync(int id, RegionMembershipRole role)
        {
            await this.GivenARegionRoleAsync($"{role.ToString()}{id}", RegionId, role);
        }

        [Fact]
        public async Task WhenIQueryMembershipsInThatRegion_ItReturnsAllTheMemberships()
        {
            var memberships = await QueryMembershipsAndAssertOKAsync(regionId: RegionId);
            Assert.Equal(6, memberships.Count());
        }
    }
}
