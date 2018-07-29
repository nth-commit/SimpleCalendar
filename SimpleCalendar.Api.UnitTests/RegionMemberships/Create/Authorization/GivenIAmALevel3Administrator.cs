using SimpleCalendar.Api.Core.Data;
using SimpleCalendar.Framework.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SimpleCalendar.Api.UnitTests.RegionMemberships.Create.Authorization
{
    public class GivenIAmALevel3Administrator : GivenAValidRegionMembership
    {
        public GivenIAmALevel3Administrator() => InitializeAsync().GetAwaiter().GetResult();

        private async Task InitializeAsync()
        {
            await this.GivenIAmARegionAdministratorAsync(Level3AdministratorId, Level3RegionId);
        }

        public new class Tests : GivenIAmALevel3Administrator
        {
            [Fact]
            public Task WhenICreateALevel1RegionUser_ThenItReturns403Unauthorized()
                => CreateUserAndAssertUnauthorizedAsync(regionLevel: 1);

            [Fact]
            public Task WhenICreateALevel2RegionUser_ThenItReturns403Unauthorized()
                => CreateUserAndAssertUnauthorizedAsync(regionLevel: 2);

            [Fact]
            public Task WhenICreateALevel3RegionUser_ThenItReturns201Created()
                => CreateUserAndAssertCreatedAsync(regionLevel: 3);

            [Fact]
            public Task WhenICreateALevel1RegionAdministrator_ThenItReturns403Unauthorized()
                => CreateAdministratorAndAssertUnauthorizedAsync(regionLevel: 1);

            [Fact]
            public Task WhenICreateALevel2RegionAdministrator_ThenItReturns403Unauthorized()
                => CreateAdministratorAndAssertUnauthorizedAsync(regionLevel: 2);

            [Fact]
            public Task WhenICreateALevel3RegionAdministrator_ThenItReturns403Unauthorized()
                => CreateAdministratorAndAssertUnauthorizedAsync(regionLevel: 3);
        }
    }
}
