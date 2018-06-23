using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SimpleCalendar.Api.UnitTests.RegionMemberships.Create.Authorization
{
    public class GivenIAmAnonymous : GivenAValidRegionMembership
    {
        public new class Tests : GivenIAmAnonymous
        {
            [Fact]
            public Task WhenICreateALevel1RegionUser_ThenItReturns403Unauthorized()
                => CreateUserAndAssertUnauthorizedAsync(regionLevel: 1);

            [Fact]
            public Task WhenICreateALevel2RegionUser_ThenItReturns403Unauthorized()
                => CreateUserAndAssertUnauthorizedAsync(regionLevel: 2);

            [Fact]
            public Task WhenICreateALevel3RegionUser_ThenItReturns403Unauthorized()
                => CreateUserAndAssertUnauthorizedAsync(regionLevel: 3);
        }
    }
}
