using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SimpleCalendar.Api.UnitTests.Events.Query.Authorization
{
    public class GivenSomeEventsFromSiblingRegions : GivenAnEventsListEndpoint
    {
        private const string Region1 = GivenAnyContextRegionExtensions.Level2ARegionId;
        private const string Region2 = GivenAnyContextRegionExtensions.Level2BRegionId;
        private const string Region1User = "Region1User";
        private const string Region2User = "Region2User";

        protected override IEnumerable<EventDefinition> EventDefinitions => new List<EventDefinition>()
        {
            new EventDefinition()
            {
                Name = "Event 1",
                RegionId = Region1,
                IsPublic = false
            },
            new EventDefinition()
            {
                Name = "Event 2",
                RegionId = Region1,
                IsPublic = false
            },
            new EventDefinition()
            {
                Name = "Event 3",
                RegionId = Region2,
                IsPublic = false
            },
            new EventDefinition()
            {
                Name = "Event 4",
                RegionId = Region2,
                IsPublic = false
            }
        };

        public class Tests : GivenSomeEventsFromSiblingRegions
        {
            [Fact]
            public async Task WhenIAmAUserInTheFirstRegion_OnlyReturnEventsFromTheFirstRegionAsync()
            {
                await this.GivenIAmARegionUserAsync(Region1User, Region1);

                var events = await Client.QueryEventsAndAssertOK();

                Assert.Equal(2, events.Count());
                Assert.Equal(new List<string>() { "Event 1", "Event 2" }, events.Select(e => e.Name));
            }

            [Fact]
            public async Task WhenIAmAUserInTheSecondRegion_OnlyReturnEventsFromTheFirstRegionAsync()
            {
                await this.GivenIAmARegionUserAsync(Region2User, Region2);

                var events = await Client.QueryEventsAndAssertOK();

                Assert.Equal(2, events.Count());
                Assert.Equal(new List<string>() { "Event 3", "Event 4" }, events.Select(e => e.Name));
            }
        }
    }
}
