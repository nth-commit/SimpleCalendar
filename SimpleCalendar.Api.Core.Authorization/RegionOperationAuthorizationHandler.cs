using Microsoft.AspNetCore.Authorization;
using SimpleCalendar.Api.Core.Data;
using SimpleCalendar.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCalendar.Api.Core.Authorization
{
    public class RegionOperationAuthorizationHandler : AuthorizationHandler<RegionOperationRequirement, RegionEntity>
    {
        private readonly IRegionPermissionResolver _regionPermissionResolver;
        private readonly IRegionRoleCache _regionRoleCache;

        public RegionOperationAuthorizationHandler(
            IRegionPermissionResolver regionPermissionResolver,
            IRegionRoleCache regionRoleCache)
        {
            _regionPermissionResolver = regionPermissionResolver;
            _regionRoleCache = regionRoleCache;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            RegionOperationRequirement requirement,
            RegionEntity resource)
        {
            if (requirement.Name == nameof(RegionOperationRequirement.CreateMembership))
            {
                var createMembershipsRequirement = requirement as RegionOperationRequirement.CreateMembershipRequirement;

                var regionRoles = await _regionRoleCache.ListAsync();
                var regionRole = regionRoles.Single(rr => rr.Id == createMembershipsRequirement.RegionRoleId);

                var requiredPermission = regionRole.IsWriter() ?
                    RegionPermission.Memberships_WriteWriter :
                    RegionPermission.Memberships_WriteReader;

                if (await _regionPermissionResolver.HasPermissionAsync(
                    context.User,
                    resource,
                    requiredPermission,
                    new Lazy<Task<IEnumerable<RegionRoleEntity>>>(
                        () => Task.FromResult(regionRoles))))
                {
                    context.Succeed(requirement);
                }
            }
            else if (requirement.Name == nameof(RegionOperationRequirement.DeleteMemberships))
            {
                if (await HasPermissionAsync(resource, RegionPermission.Memberships_WriteReader, context))
                {
                    context.Succeed(requirement);
                }
            }
            else if (requirement.Name == nameof(RegionOperationRequirement.QueryMemberships))
            {
                if (await HasPermissionAsync(resource, RegionPermission.Memberships_Read, context))
                {
                    context.Succeed(requirement);
                }
            }
        }

        private Task<bool> HasPermissionAsync(
            RegionEntity resource,
            RegionPermission permission,
            AuthorizationHandlerContext context)  =>
                _regionPermissionResolver.HasPermissionAsync(
                    context.User,
                    resource,
                    permission,
                    _regionRoleCache);
    }
}
