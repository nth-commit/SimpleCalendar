using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SimpleCalendar.Api.UnitTests.RegionMemberships.Query.My
{
    public class GivenIAmALevel2BAndLevel3AAdministrator : GivenARegionHierarchy
    {
        public GivenIAmALevel2BAndLevel3AAdministrator() => InitializeAsync().GetAwaiter().GetResult();

        private async Task InitializeAsync()
        {
            await this.GivenASuperAdministratorAsync("Administrator", Level3RegionId);
            await this.GivenASuperAdministratorAsync("Administrator", Level2BRegionId);
            this.GivenIHaveAnEmail("Administrator");
        }

        public class Tests : GivenIAmALevel2BAndLevel3AAdministrator
        {
            [Fact]
            public async Task WhenIRequestMemberships_ItReturnsRegion2BAndRegion3()
            {
                var memberships = await QueryMyMembershipsAndAssertOKAsync();
                Assert.Equal(
                    new string[] { Level2BRegionId, Level3RegionId }.OrderBy(x => x),
                    memberships.Select(m => m.RegionId).OrderBy(x => x));
            }
        }
    }
}
