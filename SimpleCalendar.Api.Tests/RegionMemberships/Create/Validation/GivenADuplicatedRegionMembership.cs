using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SimpleCalendar.Api.UnitTests.RegionMemberships.Create.Validation
{
    public class GivenADuplicatedRegionMembership : ValidationBase
    {
        public GivenADuplicatedRegionMembership() => InitializeAsync().GetAwaiter().GetResult();

        private async Task InitializeAsync()
        {
            await this.GivenARegionMembershipAsync(
                ValidRegionMembership.UserEmail,
                ValidRegionMembership.RegionId,
                ValidRegionMembership.RegionRoleId);
        }

        [Fact]
        public Task WhenICreateTheMembership_ThenItReturns400BadRequestAndValidError()
            => AssertInvalidMembershipAsync("RegionId+UserEmail", ValidRegionMembership);
    }
}
