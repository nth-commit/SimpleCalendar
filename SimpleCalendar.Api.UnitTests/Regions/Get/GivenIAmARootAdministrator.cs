using SimpleCalendar.Api.UnitTests.RegionRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SimpleCalendar.Api.UnitTests.Regions.Get
{
    public class GivenIAmARootAdministrator : GivenAGetEndpoint
    {
        public GivenIAmARootAdministrator()
        {
            this.GivenIAmARegionAdministratorAsync(
                email: "test@example.com",
                regionId: Constants.RootRegionId);
        }

        public new class Tests : GivenIAmARootAdministrator
        {
            [Fact]
            public async Task WhenIGetTheRootRegion_ICanNotAddRegionMembershipsForAdministrators()
            {
                await AssertICanAddRegionMembershipsForRoles(
                    Constants.RootRegionId,
                    new string[] { Core.Data.Constants.RegionRoles.User });
            }

            [Fact]
            public async Task WhenIGetALevel3Region_ICanAddRegionMembershipsForAllRoles()
            {
                await AssertICanAddRegionMembershipsForRoles(
                    Constants.Level3RegionId,
                    new string[]
                    {
                        Core.Data.Constants.RegionRoles.SuperAdministrator,
                        Core.Data.Constants.RegionRoles.Administrator,
                        Core.Data.Constants.RegionRoles.User
                    });
            }

            private async Task AssertICanAddRegionMembershipsForRoles(string regionId, IEnumerable<string> roles)
            {
                var regionRoles = await Client.GetRegionRolesAndAssertOKAsync();
                var region = await Client.GetRegionAndAssertOKAsync(regionId);

                var canAddMembershipPermissions = region.Permissions.CanAddMemberships;
                Assert.Equal(regionRoles.Count(), canAddMembershipPermissions.Count());
                Assert.All(canAddMembershipPermissions, kvp => Assert.Equal(kvp.Value, roles.Contains(kvp.Key)));
            }
        }
    }
}
