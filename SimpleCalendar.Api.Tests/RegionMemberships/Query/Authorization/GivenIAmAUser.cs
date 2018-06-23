using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SimpleCalendar.Api.UnitTests.RegionMemberships.Query.Authorization
{
    public class GivenIAmAUser : GivenARegionHierarchy
    {
        public GivenIAmAUser() => InitalizeAsync().GetAwaiter().GetResult();

        private Task InitalizeAsync() => this.GivenIAmARegionUserAsync("UserId", GivenAnyContextRegionExtensions.Level1RegionId);

        [Fact]
        public Task WhenIQueryMemberships_ItReturns403Unauthorized() => QueryMembershipsAndAssertUnauthorizedAsync();
    }
}
