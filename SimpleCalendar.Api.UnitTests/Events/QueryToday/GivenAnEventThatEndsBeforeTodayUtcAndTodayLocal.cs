using SimpleCalendar.Api.UnitTests.Events.Query;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SimpleCalendar.Api.UnitTests.Events.QueryToday
{
    public class GivenAnEventThatEndsBeforeTodayUtcAndTodayLocal : GivenAnEventsListEndpoint
    {
        private static readonly DateTime CurrentDateTime = new DateTime(year: 2005, month: 2, day: 20);

        public GivenAnEventThatEndsBeforeTodayUtcAndTodayLocal()
        {
            this.GivenTheCurrentUtcDateTime(CurrentDateTime);
            this.GivenIAmARootSuperAdministrator();
        }

        protected override IEnumerable<EventDefinition> EventDefinitions => new List<EventDefinition>()
        {
            new EventDefinition()
            {
                StartTime = CurrentDateTime.AddDays(-2),
                EndTime = CurrentDateTime.AddHours(-1),
                RegionId = Constants.RootRegionId
            }
        };

        [Fact]
        public async Task WhenIQueryTodayEvents_ItReturnsEmpty()
        {
            var events = await Client.QueryEventsTodayAndAssertOK(
                regionId: Constants.RootRegionId,
                timezone: "UTC-11");

            Assert.Empty(events);
        }
    }
}
