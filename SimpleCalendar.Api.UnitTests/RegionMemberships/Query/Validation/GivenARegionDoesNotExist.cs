using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SimpleCalendar.Api.UnitTests.RegionMemberships.Query.Validation
{
    public class GivenARegionDoesNotExist : ValidationBase
    {
        [Fact]
        public Task WhenIQueryTheNonExistentRegion_ThenItReturns400BadRequestAndValidError()
            => AssertInvalidQueryAsync("regionId", regionId: "not.a.region.id");
    }
}
