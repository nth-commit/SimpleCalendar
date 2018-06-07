using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SimpleCalendar.Api.UnitTests.Regions
{
    public class GivenAnEmptyDataStore : GivenAnyContext
    {

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
                var response = await Client.GetAsync("/regions?parentId=new_zealand");
                Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
                var error = await response.Content.ReadAsStringAsync();
                Assert.Equal("Parameter parentId is invalid. Could not find region with parentId \"new_zealand\".", error);
            }
        }
    }
}
