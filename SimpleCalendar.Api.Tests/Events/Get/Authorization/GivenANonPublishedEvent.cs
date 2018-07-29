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
    public class GivenANonPublishedEvent : GivenAnyContext
    {
        public const string RegionId = GivenAnyContextRegionExtensions.Level1RegionId;
        public const string CreatorUserId = "CreatorUserId";
        public static readonly Guid EventId = Guid.NewGuid();

        public GivenANonPublishedEvent()
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
                IsPublished = false,
                IsPublic = true,
                DataJson = "{}",
                DataJsonVersion = 1,
                CreatedByEmail = CreatorUserId
            });
        }

        public class Tests : GivenANonPublishedEvent
        {
            [Fact]
            public async Task WhenIAmAnonymousAndGetTheEvent_ItReturns403Unauthorized()
            {
                var response = await Client.GetEventAsync(EventId);
                response.AssertStatusCode(HttpStatusCode.NotFound);
            }

            [Fact]
            public async Task WhenIAmAUserAndGetTheEvent_ItReturns403Unauthorized()
            {
                await this.GivenIAmARegionUserAsync("UserId", RegionId);

                var response = await Client.GetEventAsync(EventId);
                response.AssertStatusCode(HttpStatusCode.NotFound);
            }

            [Fact]
            public async Task WhenIAmAnAdministratorAndGetTheEvent_ItReturns200OK()
            {
                await this.GivenIAmARegionSuperAdministratorAsync("UserId", RegionId);

                var response = await Client.GetEventAsync(EventId);
                response.AssertStatusCode(HttpStatusCode.OK);
            }

            [Fact]
            public async Task WhenIAmTheCreatorAndGetTheEvent_ItReturns200OK()
            {
                await this.GivenIAmARegionUserAsync(CreatorUserId, RegionId);

                var response = await Client.GetEventAsync(EventId);
                response.AssertStatusCode(HttpStatusCode.OK);
            }
        }
    }
}
