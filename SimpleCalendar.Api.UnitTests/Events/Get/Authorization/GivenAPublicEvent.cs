using SimpleCalendar.Api.Core.Data;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SimpleCalendar.Api.UnitTests.Events.Get.Authorization
{
    public class GivenAPublicEvent : GivenAnyContext
    {
        public const string RegionId = GivenAnyContextRegionExtensions.Level1RegionId;
        public static readonly Guid EventId = Guid.NewGuid();

        public GivenAPublicEvent()
        {
            InitializeAsync().GetAwaiter().GetResult();
        }

        private async Task InitializeAsync()
        {
            await this.GivenARegionHierarchyAsync();

            var dbContext = this.GetCoreDbContext();

            var region = await dbContext.GetRegionByIdAsync(RegionId);
            if (region == null)
            {
                throw new Exception("Region not found");
            }

            await this.GivenAnEventAsync(new EventEntity()
            {
                Id = EventId.ToString(),
                RegionId = region.Id,
                IsPublished = true,
                IsPublic = false,
                DataJson = "{}",
                DataJsonVersion = 1
            });
        }

        public class Tests : GivenAPublicEvent
        {
            [Fact]
            public async Task WhenIAmAnonymousAndGetTheEvent_ItReturns403Unauthorized()
            {
                var response = await Client.GetEventAsync(EventId);
                response.AssertStatusCode(HttpStatusCode.NotFound);
            }
        }
    }
}
