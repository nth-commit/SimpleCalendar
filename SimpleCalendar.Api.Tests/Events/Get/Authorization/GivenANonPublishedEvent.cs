﻿using SimpleCalendar.Api.Core.Data;
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
        public static readonly Guid EventId = Guid.NewGuid();

        public GivenANonPublishedEvent()
        {
            InitializeAsync().GetAwaiter().GetResult();
        }

        private async Task InitializeAsync()
        {
            await this.GivenARegionHierarchyAsync();

            var dbContext = this.GetCoreDbContext();

            var region = await dbContext.GetRegionByCodesAsync(RegionId);
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
                DataJsonVersion = 1
            });
        }

        public class Tests : GivenANonPublishedEvent
        {
            [Fact]
            public async Task GivenIAmAUserFromThatRegion_WhenIGetTheEvent_ItReturns403Unauthorized()
            {
                await this.GivenIAmARegionUserAsync("UserId", RegionId);

                var response = await Client.GetEventAsync(EventId);
                response.AssertStatusCode(HttpStatusCode.NotFound);
            }

            [Fact]
            public async Task GivenIAmAnAdministratorFromThatRegion_WhenIGetTheEvent_ItReturns200OK()
            {
                await this.GivenIAmARegionAdministratorAsync("UserId", RegionId);

                var response = await Client.GetEventAsync(EventId);
                response.AssertStatusCode(HttpStatusCode.OK);
            }
        }
    }
}
