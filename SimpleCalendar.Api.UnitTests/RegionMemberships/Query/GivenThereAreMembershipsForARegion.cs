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
            this.GivenIAmARootSuperAdministrator();

            for (var id = 0; id < RegionUserCount; id++)
            {
                await CreateMembershipAsync(id, Core.Data.Constants.RegionRoles.User);
            }

            for (var id = 0; id < RegionUserCount; id++)
            {
                await CreateMembershipAsync(id, Core.Data.Constants.RegionRoles.SuperAdministrator);
            }

            await this.GivenARegionMembershipAsync("SomeOtherUser", Level3RegionId, Core.Data.Constants.RegionRoles.User);
        }

        private async Task CreateMembershipAsync(int id, string regionRoleId)
        {
            await this.GivenARegionMembershipAsync($"{regionRoleId}_{id}", RegionId, regionRoleId);
        }

        [Fact]
        public async Task WhenIQueryMembershipsInThatRegion_ItReturnsAllTheMemberships()
        {
            var memberships = await QueryMembershipsAndAssertOKAsync(regionId: RegionId);
            Assert.Equal(6, memberships.Count());
        }
    }
}
