using Microsoft.Extensions.DependencyInjection;
using SimpleCalendar.Api.Core.Data;
using SimpleCalendar.Api.Core.Regions;
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
        private readonly IReadOnlyList<RegionCreate> _regions = new List<RegionCreate>()
        {
            new RegionCreate()
            {
                Name = "New Zealand"
            },
            new RegionCreate()
            {
                Name = "Wellington",
                ParentId = "new_zealand"
            },
            new RegionCreate()
            {
                Name = "Mount Victoria",
                ParentId = "new_zealand.wellington"
            },
            new RegionCreate()
            {
                Name = "Auckland",
                ParentId = "new_zealand"
            }
        };

        public GivenADataStoreWithARegionHierarchy()
        {
            InitializeAsync().GetAwaiter().GetResult();
        }

        private async Task InitializeAsync()
        {
            var regionService = Services.GetRequiredService<RegionService>();
            foreach (var region in _regions)
            {
                await regionService.CreateRegionAsync(region);
            }
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
