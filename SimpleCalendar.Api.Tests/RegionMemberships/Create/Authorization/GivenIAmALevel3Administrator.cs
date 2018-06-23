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
            public Task WhenICreateALevel1RegionMembership_ThenItReturns403Unauthorized()
                => CreateAndAssertUnauthorizedAsync(ValidRegionMembershipLevel1);

            [Fact]
            public Task WhenICreateALevel2RegionMembership_ThenItReturns403Unauthorized()
                => CreateAndAssertUnauthorizedAsync(ValidRegionMembershipLevel2);

            [Fact]
            public Task WhenICreateALevel3RegionMembership_ThenItReturns201Created()
                => CreateAndAssertCreatedAsync(ValidRegionMembershipLevel3);
        }
    }
}
