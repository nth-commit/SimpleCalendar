using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SimpleCalendar.Api.UnitTests.RegionMemberships.Query.Authorization
{
    public class GivenIAmAnonymous : GivenARegionHierarchy
    {
        [Fact]
        public Task WhenIQueryMemberships_ItReturns403Unauthorized() => QueryMembershipsAndAssertUnauthorizedAsync();
    }
}
