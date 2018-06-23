﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SimpleCalendar.Api.UnitTests.RegionMemberships.Create.Authorization
{
    public class GivenIAmALevel1User : GivenAValidRegionMembership
    {
        public GivenIAmALevel1User() => InitializeAsync().GetAwaiter().GetResult();

        private async Task InitializeAsync()
        {
            await this.GivenIAmARegionUserAsync("Level1User", Level1RegionId);
        }

        public new class Tests : GivenIAmALevel1User
        {
            [Fact]
            public Task WhenICreateALevel1RegionMembership_ThenItReturns403Unauthorized()
                => CreateAndAssertUnauthorizedAsync(ValidRegionMembershipLevel1);

            [Fact]
            public Task WhenICreateALevel2RegionMembership_ThenItReturns403Unauthorized()
                => CreateAndAssertUnauthorizedAsync(ValidRegionMembershipLevel2);

            [Fact]
            public Task WhenICreateALevel3RegionMembership_ThenItReturns403Unauthorized()
                => CreateAndAssertUnauthorizedAsync(ValidRegionMembershipLevel3);
        }
    }
}