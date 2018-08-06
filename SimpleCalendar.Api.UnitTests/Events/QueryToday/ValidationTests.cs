using SimpleCalendar.Api.UnitTests.Events.Query;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SimpleCalendar.Api.UnitTests.Events.QueryToday
{
    public class ValidationTests : GivenAnEmptyEventsListEndpoint
    {
        [Fact]
        public Task WhenIQueryTodayWithoutTimezone_ItReturnsOK() =>
            Client.QueryEventsTodayAndAssertOK();

        [Fact]
        public async Task WhenIQueryTodayWithInvalidTimezone_ItReturnsBadRequestAsync()
        {
            var response = await Client.GetQueryEventsTodayResponseAsync(timezone: "not-a-timezone-code");
            response.AssertStatusCodeBadRequest();
        }

        [Fact]
        public Task WhenIQueryTodayWithAValidTimezone_ItReturnsOK() =>
            Client.QueryEventsTodayAndAssertOK(timezone: "New Zealand Standard Time");
    }
}
