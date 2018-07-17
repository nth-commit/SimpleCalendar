using Microsoft.AspNetCore.Authorization;
using Moq;
using SimpleCalendar.Api.Core.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SimpleCalendar.Api.Core.Authorization.UnitTests
{
    public class GivenIHaveASetOfPermissionInTheRootRegion : GivenAnAuthorizationService
    {
        private RegionPermissionRequirement _requirement = RegionPermissionRequirement.CreateEvents;

        public GivenIHaveASetOfPermissionInTheRootRegion()
        {
            AddRegionRole("ROLE", RegionPermission.Events_All);
            AddRegionMembership(Constants.RootRegionId, "ROLE");
        }

        [Fact]
        public Task WhenIAuthorizeAgainstTheRootRegion_ItIsSuccessful() => AssertAuthorizedAsync(
            new RegionEntity()
            {
                Id = Constants.RootRegionId
            },
            _requirement);

        [Fact]
        public Task WhenIAuthorizeAgainstTheRootRegionWithADifferentRequirement_ItIsNotSuccessful() => AssertNotAuthorizedAsync(
            new RegionEntity()
            {
                Id = Constants.RootRegionId
            },
            RegionPermissionRequirement.ViewMemberships);

        [Fact]
        public Task WhenIAuthorizeAgainstALevel1Region_ItIsSuccessful() => AssertAuthorizedAsync(
            new RegionEntity()
            {
                Id = "Level1",
                Parent = new RegionEntity()
                {
                    Id = Constants.RootRegionId
                }
            },
            _requirement);
    }
}
