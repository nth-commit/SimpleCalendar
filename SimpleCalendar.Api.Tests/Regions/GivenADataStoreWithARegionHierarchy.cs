using Microsoft.Extensions.DependencyInjection;
using SimpleCalendar.Api.Core.Data;
using SimpleCalendar.Api.Core.Regions;
using SimpleCalendar.Api.UnitTests.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SimpleCalendar.Api.UnitTests.Regions
{
    public class GivenADataStoreWithARegionHierarchy : GivenAnyContext
    {
        protected const string Level1RegionId = GivenAnyContextRegionExtensions.Level1RegionId;
        protected const string Level2RegionId = GivenAnyContextRegionExtensions.Level2RegionId;
        protected const string Level2ARegionId = GivenAnyContextRegionExtensions.Level2ARegionId;
        protected const string Level2BRegionId = GivenAnyContextRegionExtensions.Level2BRegionId;
        protected const string Level3RegionId = GivenAnyContextRegionExtensions.Level3RegionId;

        public GivenADataStoreWithARegionHierarchy()
        {
            this.GivenARegionHierarchyAsync().GetAwaiter().GetResult();
        }

        protected async Task AssertRegionsReturnedAsync(string parentId, IEnumerable<string> expectedRegions)
        {
            var regions = await Client.GetRegionsAndAssertOKAsync(parentId);
            CollectionAssert.SetEqual(expectedRegions, regions.Select(r => r.Name));
        }

        public class Tests : GivenADataStoreWithARegionHierarchy
        {
            [Fact]
            public async Task WhenRegionEndpointCalledWithParent_ThenReturnChildRegions()
            {
                await AssertRegionsReturnedAsync("new-zealand", new string[] { "Auckland", "Wellington" });
            }

            [Fact]
            public async Task WhenRegionEndpointCalledWithLevelTwoParent_ThenReturnChildRegions()
            {
                await AssertRegionsReturnedAsync("new-zealand/wellington", new string[] { "Mount Victoria" });
            }

            [Fact]
            public async Task WhenRegionEndpointCalled_ThenCanGetChildRegion()
            {
                var regions = await Client.GetRegionsAndAssertOKAsync();
                var parentRegion = regions.FirstOrDefault();
                Assert.NotNull(parentRegion);

                var childRegions = await Client.GetRegionsAndAssertOKAsync(parentRegion.Id);
                Assert.NotEmpty(childRegions);
            }
        }
    }
}
