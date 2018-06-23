using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SimpleCalendar.Api.UnitTests.RegionMemberships.Create.Validation
{
    public class GivenADuplicatedRegionRole : ValidationBase
    {
        public GivenADuplicatedRegionRole() => InitializeAsync().GetAwaiter().GetResult();

        private async Task InitializeAsync()
        {
            await this.GivenARegionRoleAsync(
                ValidRegionMembership.UserId,
                ValidRegionMembership.RegionId,
                ValidRegionMembership.Role);
        }

        [Fact]
        public Task WhenICreateTheMembership_ThenItReturns400BadRequestAndValidError()
            => AssertInvalidMembershipAsync("RegionId+UserId", ValidRegionMembership);
    }
}
