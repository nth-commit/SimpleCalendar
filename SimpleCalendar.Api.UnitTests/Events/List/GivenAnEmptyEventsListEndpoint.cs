using SimpleCalendar.Api.UnitTests.Utililty;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SimpleCalendar.Api.UnitTests.Events.List
{
    public class GivenAnEmptyEventsListEndpoint : GivenAnEventsListEndpoint
    {
        protected override IEnumerable<EventDefinition> EventDefinitions => Enumerable.Empty<EventDefinition>();

        public GivenAnEmptyEventsListEndpoint()
        {
        }

        [Fact]
        public async Task WhenIAmAnonymous_ItReturns200OK()
        {
            var response = await Client.ListEventsAsync();
            response.AssertStatusCode(HttpStatusCode.OK);
        }
    }
}
