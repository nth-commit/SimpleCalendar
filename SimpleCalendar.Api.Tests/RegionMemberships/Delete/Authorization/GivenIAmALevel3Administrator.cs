using SimpleCalendar.Api.Core.Data;
using SimpleCalendar.Framework.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SimpleCalendar.Api.UnitTests.RegionMemberships.Delete.Authorization
{
    public class GivenIAmALevel3Administrator : GivenADataStoreWithExistingRegionMemberships
    {
        public GivenIAmALevel3Administrator() => InitializeAsync().GetAwaiter().GetResult();

        private async Task InitializeAsync()
        {
            await this.GivenIAmARegionAdministratorAsync("Administrator", Level3RegionId);
        }

        public new class Tests : GivenIAmALevel3Administrator
        {
            [Fact]
            public Task WhenIDeleteALevel1RegionUser_ThenItReturns403Unauthorized()
                => DeleteUserAndAssertUnauthorizedAsync(regionLevel: 1);

            [Fact]
            public Task WhenIDeleteALevel2RegionUser_ThenItReturns403Unauthorized()
                => DeleteUserAndAssertUnauthorizedAsync(regionLevel: 2);

            [Fact]
            public Task WhenIDeleteALevel3RegionUser_ThenItReturns204NoContent()
                => DeleteUserAndAssertNoContentAsync(regionLevel: 3);

            [Fact]
            public Task WhenIDeleteALevel1RegionAdministrator_ThenItReturns403Unauthorized()
                => DeleteAdministratorAndAssertUnauthorizedAsync(regionLevel: 1);

            [Fact]
            public Task WhenIDeleteALevel2RegionAdministrator_ThenItReturns403Unauthorized()
                => DeleteAdministratorAndAssertUnauthorizedAsync(regionLevel: 2);

            [Fact]
            public Task WhenIDeleteALevel3RegionAdministrator_ThenItReturns403Unauthorized()
                => DeleteAdministratorAndAssertUnauthorizedAsync(regionLevel: 3);
        }
    }
}
