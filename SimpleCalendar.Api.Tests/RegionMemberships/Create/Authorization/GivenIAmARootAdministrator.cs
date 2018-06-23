using SimpleCalendar.Api.Core.Data;
using SimpleCalendar.Framework.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SimpleCalendar.Api.UnitTests.RegionMemberships.Create.Authorization
{
    public class GivenIAmARootAdministrator : GivenAValidRegionMembership
    {
        public GivenIAmARootAdministrator() => InitializeAsync().GetAwaiter().GetResult();

        private async Task InitializeAsync()
        {
            this.GivenIAmARootAdministrator();
            await Task.CompletedTask;
        }

        public new class Tests : GivenIAmARootAdministrator
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
                => CreateAdministratorAndAssertCreatedAsync(regionLevel: 1);

            [Fact]
            public Task WhenICreateALevel2RegionAdministrator_ThenItReturns201Created()
                => CreateAdministratorAndAssertCreatedAsync(regionLevel: 2);

            [Fact]
            public Task WhenICreateALevel3RegionAdministrator_ThenItReturns201Created()
                => CreateAdministratorAndAssertCreatedAsync(regionLevel: 3);
        }
    }
}
