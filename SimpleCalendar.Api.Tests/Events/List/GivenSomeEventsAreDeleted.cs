using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SimpleCalendar.Api.UnitTests.Events.List
{
    public class GivenSomeEventsAreDeleted : GivenAnEventsListEndpoint
    {
        protected override IEnumerable<EventDefinition> EventDefinitions => new List<EventDefinition>()
        {
            new EventDefinition()
            {
                RegionId = Level1RegionId,
                IsDeleted = true,
                IsPublished = true,
                Name = "Event 1"
            },
            new EventDefinition()
            {
                RegionId = Level1RegionId,
                IsDeleted = true,
                IsPublished = true,
                Name = "Event 2"
            },
            new EventDefinition()
            {
                RegionId = Level1RegionId,
                IsDeleted = false,
                IsPublished = true,
                Name = "Event 3"
            }
        };

        public class Test : GivenSomeEventsAreDeleted
        {
            [Fact]
            public async Task WhenIAmARootAdministrator_ThenReturnAllNonDeletedEvents()
            {
                this.GivenIAmARootAdministrator();

                var response = await Client.ListEventsAsync();
                var events = await response.DeserializeEventsAsync();

                Assert.Equal(NonDeletedEvents.Count(), events.Count());
            }
        }
    }
}
