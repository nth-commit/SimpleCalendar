﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using SimpleCalendar.Api.Core.Data;
using SimpleCalendar.Framework;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SimpleCalendar.Api.Core.Authorization.UnitTests
{
    public class GivenAnAuthorizationService
    {
        private const string Email = "example@website.com";

        private readonly IAuthorizationService _authorizationService;
        private readonly Mock<IRegionRolesAccessor> _regionRolesAccessor = new Mock<IRegionRolesAccessor>();
        private readonly List<RegionRoleEntity> _regionRoleEntities = new List<RegionRoleEntity>();
        private readonly List<ClaimsExtensions.RegionMembershipRoleClaimValue> _regionMembershipRoleClaimValues = new List<ClaimsExtensions.RegionMembershipRoleClaimValue>();

        public GivenAnAuthorizationService()
        {
            var services = new ServiceCollection();
            services.AddAuthorization();
            services.AddOptions();
            services.AddLogging();
            services.AddTransient<IAuthorizationHandler, RegionPermissionAuthorizationHandler>();
            services.AddTransient<IRegionPermissionResolver, RegionPermissionResolver>();
            services.AddSingleton(_regionRolesAccessor.Object);

            _authorizationService = services.BuildServiceProvider().GetRequiredService<IAuthorizationService>();

            _regionRolesAccessor.Setup(x => x.RegionRoles).Returns(_regionRoleEntities);
        }

        protected async Task<bool> IsAuthorizedAsync(RegionEntity region, IAuthorizationRequirement requirement)
        {
            var identity = new ClaimsIdentity();
            identity.AddClaim(new Claim("sub", Guid.NewGuid().ToString()));
            identity.AddClaim(new Claim("email", Email));
            _regionMembershipRoleClaimValues.ForEach(rm => identity.AddRegionMembershipRole(rm));

            var principal = new ClaimsPrincipal(identity);
            return (await _authorizationService.AuthorizeAsync(principal, region, requirement)).Succeeded;
        }

        protected async Task AssertAuthorizedAsync(RegionEntity region, IAuthorizationRequirement requirement)
            => Assert.True(await IsAuthorizedAsync(region, requirement));

        protected async Task AssertNotAuthorizedAsync(RegionEntity region, IAuthorizationRequirement requirement)
            => Assert.False(await IsAuthorizedAsync(region, requirement));

        protected void AddRegionRole(
            string roleId,
            RegionPermission permissions,
            RegionPermission parentPermissions = RegionPermission.None,
            RegionPermission childPermissions = RegionPermission.None)
        {
            _regionRoleEntities.Add(new RegionRoleEntity()
            {
                Id = roleId,
                Permissions = permissions,
                ParentPermissions = parentPermissions,
                ChildPermissions = childPermissions
            });
        }

        protected void AddRegionMembership(string regionId, string roleId)
        {
            _regionMembershipRoleClaimValues.Add(new ClaimsExtensions.RegionMembershipRoleClaimValue()
            {
                RegionId = regionId,
                RegionRoleId = roleId
            });
        }
    }
}
