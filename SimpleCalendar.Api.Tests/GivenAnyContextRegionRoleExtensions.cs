using SimpleCalendar.Api.Core.Data;
using SimpleCalendar.Api.Models;
using SimpleCalendar.Framework.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCalendar.Api.UnitTests
{
    public static class GivenAnyContextRegionRoleExtensions
    {
        public static Task<RegionRoleEntity> GivenAnAdministratorAsync(
            this GivenAnyContext context, string userId, string regionId)
                => context.GivenARegionRoleAsync(userId, regionId, RegionMembershipRole.Administrator);

        public static async Task<RegionRoleEntity> GivenARegionRoleAsync(
            this GivenAnyContext context,
            string userId,
            string regionId,
            RegionMembershipRole role)
        {
            var coreDbContext = context.GetCoreDbContext();

            var region = await coreDbContext.GetRegionByCodesAsync(regionId);
            if (region == null)
            {
                throw new Exception("Region does not exist");
            }

            var result = await coreDbContext.RegionRoles.AddAsync(new RegionRoleEntity()
            {
                UserId = userId,
                RegionId = region.Id,
                Role = (Role)role
            });
            await coreDbContext.SaveChangesAsync();

            return result.Entity;
        }
    }
}
