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
