using SimpleCalendar.Api.Core.Events;
using SimpleCalendar.Api.UnitTests.Regions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SimpleCalendar.Api.UnitTests.Events.Create
{
    public class GivenAValidEvent : GivenIAmARootAdministrator
    {
        protected const string Level1RegionId = GivenAnyContextRegionExtensions.Level1RegionId;
        protected const string Level2RegionId = GivenAnyContextRegionExtensions.Level2RegionId;
        protected const string Level3RegionId = GivenAnyContextRegionExtensions.Level3RegionId;

        protected static readonly DateTime CurrentTime = new DateTime(2010, 10, 10);

        protected static readonly EventCreate ValidEvent = new EventCreate()
        {
            Name = "My Event!",
            Description = "This is my event!",
            RegionId = Level3RegionId,
            StartTime = CurrentTime.AddDays(5).AddHours(6),
            EndTime = CurrentTime.AddDays(5).AddHours(12)
        };

        public GivenAValidEvent()
        {
            this.CreateRegionHierarchyAsync().GetAwaiter().GetResult();
        }

        public class Tests : GivenAValidEvent
        {
            [Fact]
            public async Task WhenICreateAnEvent_ThenItReturns201CreatedAsync()
            {
                var response = await Client.CreateEventAsync(ValidEvent);
                Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            }
        }
    }
}
