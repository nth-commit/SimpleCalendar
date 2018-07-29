using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SimpleCalendar.Api.UnitTests.RegionMemberships.Delete.Authorization
{
    public class GivenIAmAUser : GivenADataStoreWithExistingRegionMemberships
    {
        public GivenIAmAUser() => InitializeAsync().GetAwaiter().GetResult();

        private async Task InitializeAsync()
        {
            await this.GivenIAmARegionUserAsync("Level1User", Level1RegionId);
        }

        public class Tests : GivenIAmAUser
        {
            [Fact]
            public Task WhenIDeleteALevel1RegionUser_ThenItReturns403Unauthorized()
                => DeleteUserAndAssertUnauthorizedAsync(regionLevel: 1);

            [Fact]
            public Task WhenIDeleteALevel2RegionUser_ThenItReturns403Unauthorized()
                => DeleteUserAndAssertUnauthorizedAsync(regionLevel: 2);

            [Fact]
            public Task WhenIDeleteALevel3RegionUser_ThenItReturns403Unauthorized()
                => DeleteUserAndAssertUnauthorizedAsync(regionLevel: 3);
        }
    }
}
