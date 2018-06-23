using SimpleCalendar.Api.Core.Data;
using SimpleCalendar.Framework.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SimpleCalendar.Api.UnitTests.RegionMemberships.Create.Authorization
{
    public class GivenIAmALevel1Administrator : GivenAValidRegionMembership
    {
        public GivenIAmALevel1Administrator() => InitializeAsync().GetAwaiter().GetResult();

        private async Task InitializeAsync()
        {
            await this.GivenIAmARegionAdministratorAsync(Level1AdministratorId, Level1RegionId);
        }

        public new class Tests : GivenIAmALevel1Administrator
        {
            [Fact]
            public Task WhenICreateALevel1RegionUser_ThenItReturns201Created()
                => CreateUserAndAssertCreatedAsync(regionLevel: 1);

            [Fact]
            public Task WhenICreateALevel2RegionUser_ThenItReturns201Created()
                => CreateUserAndAssertCreatedAsync(regionLevel: 2);

            [Fact]
            public Task WhenICreateALevel3RegionUser_ThenItReturns201Created()
                => CreateUserAndAssertCreatedAsync(regionLevel: 3);

            [Fact]
            public Task WhenICreateALevel1RegionAdministrator_ThenItReturns201Created()
                => CreateAdministratorAndAssertUnauthorizedAsync(regionLevel: 1);

            [Fact]
            public Task WhenICreateALevel2RegionAdministrator_ThenItReturns201Created()
                => CreateAdministratorAndAssertCreatedAsync(regionLevel: 2);

            [Fact]
            public Task WhenICreateALevel3RegionAdministrator_ThenItReturns201Created()
                => CreateAdministratorAndAssertCreatedAsync(regionLevel: 3);
        }
    }
}
