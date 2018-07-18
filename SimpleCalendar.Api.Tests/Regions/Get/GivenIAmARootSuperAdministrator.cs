using SimpleCalendar.Api.UnitTests.RegionRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SimpleCalendar.Api.UnitTests.Regions.Get
{
    public class GivenIAmARootSuperAdministrator : GivenAGetEndpoint
    {
        public GivenIAmARootSuperAdministrator()
        {
            this.GivenIAmARootSuperAdministrator();
        }

        public new class Tests : GivenIAmARootSuperAdministrator
        {
            [Fact]
            public async Task WhenIGetTheRootRegion_ICanAddRegionMembershipsForAllRoles()
            {
                await AssertICanAddRegionMembershipsForAllRoles(Constants.RootRegionId);
            }

            [Fact]
            public async Task WhenIGetALevel3Region_ICanAddRegionMembershipsForAllRoles()
            {
                await AssertICanAddRegionMembershipsForAllRoles(Constants.Level3RegionId);
            }

            private async Task AssertICanAddRegionMembershipsForAllRoles(string regionId)
            {
                var regionRoles = await Client.GetRegionRolesAndAssertOKAsync();
                var region = await Client.GetRegionAndAssertOKAsync(regionId);

                var canAddMembershipPermissions = region.Permissions.CanAddMemberships;
                Assert.Equal(regionRoles.Count(), canAddMembershipPermissions.Count());
                Assert.All(canAddMembershipPermissions, kvp => Assert.True(kvp.Value));
            }
        }
    }
}
