using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleCalendar.Api.UnitTests.Events.Query;
using Xunit;

namespace SimpleCalendar.Api.UnitTests.Events.QueryMy
{
    public class GivenSomeEventsCreatedByTwoUsers : GivenAnEventsListEndpoint
    {
        protected const string RegionId = GivenAnyContextRegionExtensions.Level1RegionId;
        protected const string FirstUserEmail = "TheFirstUser";
        protected const string FirstEventName = "FirstEvent";
        protected const string SecondUserEmail = "TheSecondUser";
        protected const string SecondEventName = "SecondEvent";

        protected override IEnumerable<EventDefinition> EventDefinitions => new EventDefinition[]
        {
            new EventDefinition()
            {
                CreatedById = FirstUserEmail,
                RegionId = RegionId,
                Name = FirstEventName
            },
            new EventDefinition()
            {
                CreatedById = SecondUserEmail,
                RegionId = RegionId,
                Name = SecondEventName 
            }
        };

        [Fact]
        public async Task GivenIAmTheFirstUser_WhenIQueryMyEvents_ThenItReturnsOnlyMyEvents()
        {
            this.GivenIHaveAnEmail(FirstUserEmail);
            var events = await Client.QueryMyEventsAndAssertOK(RegionId);
            Assert.Single(events);
            Assert.Equal(FirstEventName, events.Single().Name);
        }

        [Fact]
        public async Task GivenIAmTheSecondUser_WhenIQueryMyEvents_ThenItReturnsOnlyMyEvents()
        {
            this.GivenIHaveAnEmail(SecondUserEmail);
            var events = await Client.QueryMyEventsAndAssertOK(RegionId);
            Assert.Single(events);
            Assert.Equal(SecondEventName, events.Single().Name);
        }
    }
}
