using SimpleCalendar.Api.Core.Events;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SimpleCalendar.Api.UnitTests.Events.Create.Invalid
{
    public class GivenAnEventWithAnInvalidName : GivenAValidEvent
    {
        public GivenAnEventWithAnInvalidName()
        {
        }

        [Fact]
        public async Task WhenICreateAnEvent_ThenItReturns400WithSpecificError()
        {
            var invalidEvent = ValidEvent;
            invalidEvent.Name = null;

            var response = await Client.CreateEventAsync(invalidEvent);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            var result = await response.Content.ReadAsStringAsync();
        }
    }
}
