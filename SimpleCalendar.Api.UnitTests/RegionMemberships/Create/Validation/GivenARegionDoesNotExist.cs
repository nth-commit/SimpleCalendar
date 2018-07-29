using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SimpleCalendar.Api.UnitTests.RegionMemberships.Create.Validation
{
    public class GivenARegionDoesNotExist : ValidationBase
    {
        [Fact]
        public Task WhenICreateTheMembership_ThenItReturns400BadRequestAndValidError()
            => AssertInvalidMembershipAsync("RegionId", membership => membership.RegionId = "not.a.region.id");
    }
}
