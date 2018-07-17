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
    public class GivenIHaveAChildPermissionForALevel1Region : GivenAnAuthorizationService
    {
        private readonly RegionPermissionRequirement _requirement = RegionPermissionRequirement.CreateWriterMemberships;

        public GivenIHaveAChildPermissionForALevel1Region()
        {
            AddRegionRole(
                "ROLE",
                RegionPermission.None,
                childPermissions: _requirement.Permission);

            AddRegionMembership("Level1", "ROLE");
        }

        [Fact]
        public Task WhenIAuthorizeAgainstTheLevel1Region_ItIsNotSuccessful() => AssertNotAuthorizedAsync(
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
        public Task WhenIAuthorizeAgainstALevel2Region_ItIsSuccessful() => AssertAuthorizedAsync(
            new RegionEntity()
            {
                Id = "Level2",
                Parent = new RegionEntity()
                {
                    Id = "Level1",
                    Parent = new RegionEntity()
                    {
                        Id = Constants.RootRegionId
                    }
                },
            },
            _requirement);
    }
}
