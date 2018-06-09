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
        public GivenADataStoreWithARegionHierarchy()
        {
            this.GivenARegionHierarchyAsync().GetAwaiter().GetResult();
        }

        public class Tests : GivenADataStoreWithARegionHierarchy
        {
            [Fact]
            public async Task WhenRegionEndpointCalledWithParent_ThenReturnChildRegions()
            {
                await AssertRegionsReturnedAsync("/regions?parentId=new_zealand", new string[] {
                    "Auckland",
                    "Wellington"
                });
            }

            [Fact]
            public async Task WhenRegionEndpointCalledWithLevelTwoParent_ThenReturnChildRegions()
            {
                await AssertRegionsReturnedAsync("/regions?parentId=new_zealand.wellington", new string[] {
                    "Mount Victoria"
                });
            }

            private async Task AssertRegionsReturnedAsync(string query, IEnumerable<string> expectedRegions)
            {
                var response = await Client.GetAsync(query);
                var regions = await response.DeserializeRegionsAsync();
                CollectionAssert.SetEqual(expectedRegions, regions.Select(r => r.Name));
            }
        }
    }
}
