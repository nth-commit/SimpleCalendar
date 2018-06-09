﻿using SimpleCalendar.Api.Core.Data;
using SimpleCalendar.Framework.Identity;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SimpleCalendar.Api.UnitTests.Events.Create.Authorization
{
    public class GivenIAmALevel2Administrator : GivenAValidEvent
    {
        public const string Level2AdministratorId = "Level2Administrator";

        public GivenIAmALevel2Administrator()
        {
            InitializeAsync().GetAwaiter().GetResult();
        }

        private async Task InitializeAsync()
        {
            var dbContext = this.GetRequiredService<CoreDbContext>();

            await dbContext.RegionRoles.AddAsync(new RegionRoleEntity()
            {
                RegionId = Level2RegionId,
                Role = Role.Administrator,
                UserId = Level2AdministratorId
            });

            await dbContext.SaveChangesAsync();

            this.GivenIHaveAUserId(Level2AdministratorId);
        }

        public new class Tests : GivenIAmALevel2Administrator
        {
            [Fact]
            public async Task WhenICreateAnEventInALevel1Region_ThenItReturns403Unauthorized()
            {
                ValidEvent.RegionId = Level1RegionId;
                var response = await Client.CreateEventAsync(ValidEvent);
                Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
            }
        }
    }
}