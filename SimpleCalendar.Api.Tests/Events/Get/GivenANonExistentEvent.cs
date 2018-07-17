using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SimpleCalendar.Api.UnitTests.Events.Get
{
    public class GivenANonExistentEvent : GivenAnyContext
    {
        public static readonly Guid NonExistentEventId = Guid.NewGuid();

        public GivenANonExistentEvent()
        {
        }

        public class Tests : GivenANonExistentEvent
        {
            public Tests()
            {
                this.GivenIAmARootSuperAdministrator();
            }

            [Fact]
            public async Task WhenIGetANonExistentEvent_ThenItReturns404NotFound()
            {
                var response = await Client.GetEventAsync(NonExistentEventId);
                response.AssertStatusCode(HttpStatusCode.NotFound);
            }
        }
    }
}
