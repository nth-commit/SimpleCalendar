using SimpleCalendar.Api.UnitTests.Events.Query;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SimpleCalendar.Api.UnitTests.Events.QueryToday
{
    public class GivenAnEventThatEndedBeforeToday : GivenAnEventsListEndpoint
    {
        private static readonly DateTime CurrentDateTime = new DateTime(year: 2005, month: 2, day: 20);

        public GivenAnEventThatEndedBeforeToday()
        {
            this.GivenTheCurrentUtcDateTime(CurrentDateTime);
            this.GivenIAmARootSuperAdministrator();
        }

        protected override IEnumerable<EventDefinition> EventDefinitions => new List<EventDefinition>()
        {
            new EventDefinition()
            {
                StartTime = CurrentDateTime.AddDays(-2),
                EndTime = CurrentDateTime.AddDays(-1),
                RegionId = Constants.RootRegionId
            }
        };

        [Fact]
        public async Task WhenIGetTheEvents_ItReturnsEmpty()
        {
            var events = await Client.QueryEventsTodayAndAssertOK(Constants.RootRegionId);
            Assert.Empty(events);
        }
    }
}
