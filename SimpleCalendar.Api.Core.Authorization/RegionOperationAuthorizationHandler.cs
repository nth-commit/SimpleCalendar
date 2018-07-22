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
                var regionRoleId = ((RegionOperationRequirement.CreateMembershipRequirement)requirement).RegionRoleId;
                var requiredPermission = GetRequiredMembershipWritePermission(regionRoleId);

                if (HasPermission(resource, requiredPermission, context))
                {
                    context.Succeed(requirement);
                }
            }
            else if (requirement.Name == nameof(RegionOperationRequirement.DeleteMemberships))
            {
                var deleteRequirement = (RegionOperationRequirement.DeleteMembershipRequirement)requirement;
                var regionRoleId = deleteRequirement.RegionRoleId;
                var requiredPermission = GetRequiredMembershipWritePermission(regionRoleId);

                if (HasPermission(resource, requiredPermission, context) &&
                    deleteRequirement.UserEmail != context.User.GetUserEmail())
                {
                    context.Succeed(requirement);
                }
            }
            else if (requirement.Name == nameof(RegionOperationRequirement.QueryMemberships))
            {
                if (HasPermission(resource, RegionPermission.Memberships_Read, context))
                {
                    context.Succeed(requirement);
                }
            }
            return Task.CompletedTask;
        }

        private RegionPermission GetRequiredMembershipWritePermission(string regionRoleId)
        {
            var regionRoles = _regionRolesAccessor.RegionRoles;
            var regionRole = regionRoles.Single(rr => rr.Id == regionRoleId);

            return regionRole.IsWriter() ?
                RegionPermission.Memberships_Write_Writer :
                RegionPermission.Memberships_Write_Reader;
        }

        private bool HasPermission(
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
