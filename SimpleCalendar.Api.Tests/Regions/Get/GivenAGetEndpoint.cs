using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SimpleCalendar.Api.UnitTests.Regions.Get
{
    public class GivenAGetEndpoint : GivenADataStoreWithARegionHierarchy
    {

        public GivenAGetEndpoint()
        {
        }

        public new class Tests : GivenAGetEndpoint
        {
            [Fact]
            public async Task WhenIGetLevel1Region_ThenItReturns200OK()
            {
                var region = await Client.GetRegionAndAssertOKAsync(Level1RegionId);
                Assert.Equal(Level1RegionId, region.Id);
            }

            [Fact]
            public async Task WhenIGetLevel2Region_ThenItReturns200OK()
            {
                var region = await Client.GetRegionAndAssertOKAsync(Level2RegionId);
                Assert.Equal(Level2RegionId, region.Id);
            }
        }
    }
}
