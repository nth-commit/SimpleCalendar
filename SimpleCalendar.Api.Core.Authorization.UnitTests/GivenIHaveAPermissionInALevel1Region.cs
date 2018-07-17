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
    public class GivenIHaveAPermissionInALevel1Region : GivenAnAuthorizationService
    {
        private RegionPermissionRequirement _requirement = RegionPermissionRequirement.CreateEvents;

        public GivenIHaveAPermissionInALevel1Region()
        {
            AddRegionRole("ROLE", _requirement.Permission);
            AddRegionMembership("Level1", "ROLE");
        }

        [Fact]
        public Task WhenIAuthorizeAgainstTheRootRegion_ItIsNotSuccessful() => AssertNotAuthorizedAsync(
            new RegionEntity()
            {
                Id = Constants.RootRegionId
            },
            _requirement);

        [Fact]
        public Task WhenIAuthorizeAgainstTheLevel1Region_ItIsSuccessful() => AssertAuthorizedAsync(
            new RegionEntity()
            {
                Id = "Level1",
                Parent = new RegionEntity()
                {
                    Id = Constants.RootRegionId
                }
            },
            _requirement);

        [Fact]
        public Task WhenIAuthorizeAgainstTheLevel1RegionWithADifferentRequirement_ItIsNotSuccessful() => AssertNotAuthorizedAsync(
            new RegionEntity()
            {
                Id = "Level1",
                Parent = new RegionEntity()
                {
                    Id = Constants.RootRegionId
                }
            },
            RegionPermissionRequirement.PublishEvents);
    }
}
