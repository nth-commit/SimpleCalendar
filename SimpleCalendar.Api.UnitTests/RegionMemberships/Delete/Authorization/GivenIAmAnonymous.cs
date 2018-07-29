using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SimpleCalendar.Api.UnitTests.RegionMemberships.Delete.Authorization
{
    public class GivenIAmAnonymous : GivenADataStoreWithExistingRegionMemberships
    {
        [Fact]
        public Task WhenIDeleteNonExistingMembership_ThenItReturns403Unauthorized()
            => DeleteAndAssertUnauthorizedAsync("not.a.valid.membership.id");
    }
}
