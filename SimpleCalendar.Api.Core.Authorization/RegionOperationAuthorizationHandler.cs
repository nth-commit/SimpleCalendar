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
        private readonly IRegionRolesAccessor _regionRolesAccessor;

        public RegionOperationAuthorizationHandler(
            IRegionPermissionResolver regionPermissionResolver,
            IRegionRolesAccessor regionRolesAccessor)
        {
            _regionPermissionResolver = regionPermissionResolver;
            _regionRolesAccessor = regionRolesAccessor;
        }

        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            RegionOperationRequirement requirement,
            RegionEntity resource)
        {
            if (requirement.Name == nameof(RegionOperationRequirement.CreateMembership))
            {
                var createMembershipsRequirement = requirement as RegionOperationRequirement.CreateMembershipRequirement;

                var regionRoles = _regionRolesAccessor.RegionRoles;
                var regionRole = regionRoles.Single(rr => rr.Id == createMembershipsRequirement.RegionRoleId);

                var requiredPermission = regionRole.IsWriter() ?
                    RegionPermission.Memberships_Write_Writer :
                    RegionPermission.Memberships_Write_Reader;

                if (_regionPermissionResolver.HasPermission(
                    context.User,
                    resource,
                    requiredPermission,
                    regionRoles))
                {
                    context.Succeed(requirement);
                }
            }
            else if (requirement.Name == nameof(RegionOperationRequirement.DeleteMemberships))
            {
                if (HasPermissionAsync(resource, RegionPermission.Memberships_Write_Reader, context))
                {
                    context.Succeed(requirement);
                }
            }
            else if (requirement.Name == nameof(RegionOperationRequirement.QueryMemberships))
            {
                if (HasPermissionAsync(resource, RegionPermission.Memberships_Read, context))
                {
                    context.Succeed(requirement);
                }
            }
            return Task.CompletedTask;
        }

        private bool HasPermissionAsync(
            RegionEntity resource,
            RegionPermission permission,
            AuthorizationHandlerContext context)  =>
                _regionPermissionResolver.HasPermission(
                    context.User,
                    resource,
                    permission,
                    _regionRolesAccessor.RegionRoles);
    }
}
