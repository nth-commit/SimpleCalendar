using Microsoft.EntityFrameworkCore;
using SimpleCalendar.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SimpleCalendar.Api.UnitTests.RegionRoles
{
    public class GivenARegionRolesEndpoint : GivenAnyContext
    {
        public GivenARegionRolesEndpoint() => InitializeAsync().GetAwaiter().GetResult();

        private Task InitializeAsync() => this.GivenARegionHierarchyAsync();

        [Fact]
        public async Task GivenIAmARootSuperAdministrator_WhenIGetRegionRoles_ItReturnsAllTheRoles()
        {
            this.GivenIAmARootSuperAdministrator();
            var roles = await Client.GetRegionRolesAndAssertOKAsync();
            await AssertRoles(roles);
        }

        [Fact]
        public async Task GivenIAmALevel3SuperAdministrator_WhenIGetRegionRoles_ItReturnsAllTheRoles()
        {
            await this.GivenIAmARegionSuperAdministratorAsync(
                email: "test@example.com",
                regionId: GivenAnyContextRegionExtensions.Level3RegionId);
            var roles = await Client.GetRegionRolesAndAssertOKAsync();
            await AssertRoles(roles);
        }

        [Fact]
        public async Task GivenIAmARootAdministrator_WhenIGetRegionRoles_ItReturnsAllTheRoles()
        {
            await this.GivenIAmARegionAdministratorAsync(
                email: "test@example.com",
                regionId: Constants.RootRegionId);
            var roles = await Client.GetRegionRolesAndAssertOKAsync();
            await AssertRoles(roles);
        }

        [Fact]
        public async Task GivenIAmALevel3Administrator_WhenIGetRegionRoles_ItReturnsAllTheRoles()
        {
            await this.GivenIAmARegionAdministratorAsync(
                email: "test@example.com",
                regionId: GivenAnyContextRegionExtensions.Level3RegionId);
            var roles = await Client.GetRegionRolesAndAssertOKAsync();
            await AssertRoles(roles);
        }

        [Fact]
        public async Task GivenIAmARootUser_WhenIGetRegionRoles_ItReturnsUnauthorized()
        {
            await this.GivenIAmARegionUserAsync(
                email: "test@example.com",
                regionId: Constants.RootRegionId);
            await Client.GetRegionRolesAndAssertUnauthorizedAsync();
        }

        private async Task AssertRoles(IEnumerable<RegionRole> actual)
        {
            var expected = await this.GetCoreDbContext().RegionRoles.ToListAsync();
            Assert.Equal(expected.Count(), actual.Count());
            Assert.Equal(expected.Select(rr => rr.Id).OrderBy(x => x), actual.Select(rr => rr.Id).OrderBy(x => x));
        }
    }
}
