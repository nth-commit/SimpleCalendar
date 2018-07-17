using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using SimpleCalendar.Api.Core.Data;
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
        private readonly Mock<IRegionRoleCache> _regionRoleCache = new Mock<IRegionRoleCache>();
        private readonly Mock<IRegionMembershipCache> _regionMembershipCache = new Mock<IRegionMembershipCache>();
        private readonly List<RegionRoleEntity> _regionRoleEntities = new List<RegionRoleEntity>();
        private readonly List<RegionMembershipEntity> _regionMembershipEntities = new List<RegionMembershipEntity>();

        public GivenAnAuthorizationService()
        {
            var services = new ServiceCollection();
            services.AddAuthorization();
            services.AddOptions();
            services.AddLogging();
            services.AddTransient<IAuthorizationHandler, RegionPermissionAuthorizationHandler>();
            services.AddTransient<IRegionPermissionResolver, RegionPermissionResolver>();
            services.AddSingleton(_regionRoleCache.Object);
            services.AddSingleton(_regionMembershipCache.Object);

            _authorizationService = services.BuildServiceProvider().GetRequiredService<IAuthorizationService>();

            _regionRoleCache.Setup(x => x.ListAsync()).ReturnsAsync(_regionRoleEntities);
            _regionMembershipCache.Setup(x => x.ListRegionMembershipsAsync(It.IsAny<string>())).ReturnsAsync(_regionMembershipEntities);
        }

        protected async Task<bool> IsAuthorizedAsync(RegionEntity region, IAuthorizationRequirement requirement)
        {
            var identity = new ClaimsIdentity();
            identity.AddClaim(new Claim("sub", Guid.NewGuid().ToString()));
            identity.AddClaim(new Claim("email", Email));
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
            _regionMembershipEntities.Add(new RegionMembershipEntity()
            {
                RegionId = regionId,
                RegionRoleId = roleId,
                UserEmail = Email
            });
        }
    }
}
