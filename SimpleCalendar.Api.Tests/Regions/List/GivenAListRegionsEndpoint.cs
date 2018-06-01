using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SimpleCalendar.Api.UnitTests.Regions.List
{
    public class GivenAListRegionsEndpoint : GivenAnyContext
    {
        public GivenAListRegionsEndpoint()
        {
        }

        [Fact]
        public async Task WhenCalledWithNoQuery_ThenReturnsAllRegions()
        {
            var response = await Client.GetAsync("/regions");
        }
    }
}
