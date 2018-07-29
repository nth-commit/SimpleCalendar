using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SimpleCalendar.Api.UnitTests.Regions
{
    public class GivenAnEmptyDataStore : GivenAnyContext
    {
        public GivenAnEmptyDataStore() => InitializeAsync().GetAwaiter().GetResult();

        private async Task InitializeAsync()
        {
            var coreDbContext = this.GetCoreDbContext();

            var regions = await coreDbContext.Regions
                .Where(r => r.Id != Constants.RootRegionId)
                .ToListAsync();

            foreach (var region in regions)
            {
                coreDbContext.Regions.Remove(region);
            }

            await coreDbContext.SaveChangesAsync();
        }

        public class Tests : GivenAnEmptyDataStore
        {
            [Fact]
            public async Task WhenRegionsEndpointCalled_ThenReturnsEmptyResult()
            {
                var response = await Client.GetAsync("/regions");
                var regions = await response.DeserializeRegionsAsync();
                Assert.Empty(regions);
            }

            [Fact]
            public async Task WhenRegionsEndpointCalledWithParentId_ThenReturns400()
            {
                var response = await Client.GetAsync("/regions?parentId=new-zealand");
                Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            }
        }
    }
}
