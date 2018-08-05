using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SimpleCalendar.Api.UnitTests.Events.Query.Dates
{
    public class GivenAnEventWithADate : GivenAnEventsListEndpoint
    {
        private static readonly DateTime FromDate = new DateTime(2000, 1, 1);
        private static readonly DateTime ToDate = new DateTime(2000, 1, 2);

        protected override IEnumerable<EventDefinition> EventDefinitions => new List<EventDefinition>()
        {
            new EventDefinition()
            {
                RegionId = Level1RegionId,
                StartTime = FromDate,
                EndTime = ToDate
            }
        };

        public class Tests : GivenAnEventWithADate
        {
            [Fact]
            public async Task WhenIAskForEventsToBeforeThatDate_ItReturnsNoEvents()
            {
                var events = await Client.QueryEventsAndAssertOK(toDate: FromDate.AddDays(-1));

                Assert.Empty(events);
            }

            [Fact]
            public async Task WhenIAskForEventsFromAfterThatDate_ItReturnsNoEvents()
            {
                var events = await Client.QueryEventsAndAssertOK(fromDate: ToDate.AddDays(1));

                Assert.Empty(events);
            }

            [Fact]
            public async Task WhenIAskForEventsMatchingThoseDates_ItReturnsTheEvent()
            {
                var events = await Client.QueryEventsAndAssertOK(fromDate: FromDate, toDate: ToDate);

                Assert.NotEmpty(events);
            }

            [Fact]
            public async Task WhenIAskForEventsOutsideTheEventsDateRange_ItReturnsTheEvent()
            {
                var events = await Client.QueryEventsAndAssertOK(fromDate: FromDate.AddDays(-1), toDate: ToDate.AddDays(1));

                Assert.NotEmpty(events);
            }

            [Fact(Skip = "Known defect")]
            public async Task WhenIAskForEventsInsideTheEventsDateRange_ItReturnsTheEvent()
            {
                var events = await Client.QueryEventsAndAssertOK(fromDate: FromDate.AddHours(6), toDate: ToDate.AddHours(-6));

                Assert.NotEmpty(events);
            }
        }
    }
}
