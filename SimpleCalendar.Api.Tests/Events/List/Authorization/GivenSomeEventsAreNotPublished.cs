using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SimpleCalendar.Api.UnitTests.Events.List.Authorization
{
    public class GivenSomeEventsAreNotPublished : GivenAnEventsListEndpoint
    {
        private const string _nonPublishedEventName = "Non-Published Event";
        private const string _publishedEventName = "Published Event";
        private const string _creatorId = "TheCreator";

        protected override IEnumerable<EventDefinition> EventDefinitions => new List<EventDefinition>()
        {
            new EventDefinition()
            {
                RegionId = Level2RegionId,
                IsPublished = false,
                CreatedById = _creatorId,
                Name = _nonPublishedEventName
            },
            new EventDefinition()
            {
                RegionId = Level2RegionId,
                IsPublished = true,
                Name = _publishedEventName
            }
        };

        [Fact]
        public async Task WhenIAmAnAnonymousUser_ReturnThePublishedEvent()
        {
            var response = await Client.ListEventsAsync();
            response.AssertStatusCode(HttpStatusCode.OK);

            var events = await response.DeserializeEventsAsync();
            Assert.Single(events);
            Assert.Equal(_publishedEventName, events.Single().Name);
        }

        [Fact]
        public async Task WhenIAmAUser_ReturnThePublishedEvent()
        {
            await this.GivenIAmARegionUserAsync("UserId", Level2RegionId);

            var response = await Client.ListEventsAsync();
            response.AssertStatusCode(HttpStatusCode.OK);

            var events = await response.DeserializeEventsAsync();
            Assert.Single(events);
            Assert.Equal(_publishedEventName, events.Single().Name);
        }

        [Fact]
        public async Task WhenIAmAChildAdministrator_ReturnThePublishedEvent()
        {
            await this.GivenIAmARegionAdministratorAsync("Level3RegionAdministrator", Level3RegionId);

            var response = await Client.ListEventsAsync();
            response.AssertStatusCode(HttpStatusCode.OK);

            var events = await response.DeserializeEventsAsync();
            Assert.Single(events);
            Assert.Equal(_publishedEventName, events.Single().Name);
        }

        [Fact]
        public async Task WhenIAmTheCreator_ReturnAllEvents()
        {
            await this.GivenIAmARegionUserAsync(_creatorId, Level2RegionId);

            var response = await Client.ListEventsAsync();
            response.AssertStatusCode(HttpStatusCode.OK);

            var events = await response.DeserializeEventsAsync();
            Assert.Equal(EventDefinitions.Count(), events.Count());
        }

        [Fact]
        public async Task WhenIAmAnAdministrator_ReturnAllEvents()
        {
            await this.GivenIAmARegionAdministratorAsync(_creatorId, Level2RegionId);

            var response = await Client.ListEventsAsync();
            response.AssertStatusCode(HttpStatusCode.OK);

            var events = await response.DeserializeEventsAsync();
            Assert.Equal(EventDefinitions.Count(), events.Count());
        }
    }
}
