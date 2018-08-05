using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SimpleCalendar.Api.UnitTests.Events.Query
{
    public class ValidationTests : GivenAnEmptyEventsListEndpoint
    {
        [Fact]
        public async Task WhenIQueryWithoutARegionId_ThenItReturnsBadRequest()
        {
            var response = await Client.GetQueryEventsResponseAsync(regionId: null);
            response.AssertStatusCodeBadRequest();
        }
    }
}
