using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using SimpleCalendar.Api.Core.Data;
using SimpleCalendar.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SimpleCalendar.Api.Services
{
    public class AddRolesClaimTransformation : IClaimsTransformation
    {
        private CoreDbContext _coreDbContext;

        public AddRolesClaimTransformation(
            CoreDbContext coreDbContext)
        {
            _coreDbContext = coreDbContext;
        }

        public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            var identity = new ClaimsIdentity(principal.Identity);

            var regionMemberships = await _coreDbContext.RegionMemberships
                .Where(rm => rm.UserEmail == principal.GetUserEmail())
                .ToListAsync();

            foreach (var regionMembership in regionMemberships)
            {
                identity.AddRegionMembershipRole(new ClaimsExtensions.RegionMembershipRoleClaimValue()
                {
                    RegionId = regionMembership.RegionId,
                    RegionRoleId = regionMembership.RegionRoleId
                });
            }

            return new ClaimsPrincipal(identity);
        }
    }
}
