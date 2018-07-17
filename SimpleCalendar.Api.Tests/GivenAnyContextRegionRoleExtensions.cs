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
        public static Task<RegionMembershipEntity> GivenASuperAdministratorAsync(
            this GivenAnyContext context, string userId, string regionId)
                => context.GivenARegionMembershipAsync(userId, regionId, Constants.RegionRoles.SuperAdministrator);

        public static async Task<RegionMembershipEntity> GivenARegionMembershipAsync(
            this GivenAnyContext context,
            string email,
            string regionId,
            string regionRoleId)
        {
            var coreDbContext = context.GetCoreDbContext();

            var region = await coreDbContext.GetRegionByCodesAsync(regionId);
            if (region == null)
            {
                throw new Exception("Region does not exist");
            }

            var regionRole = await coreDbContext.RegionRoles.FindAsync(regionRoleId);
            if (regionRole == null)
            {
                throw new Exception("Region role does not exist");
            }

            var result = await coreDbContext.RegionMemberships.AddAsync(new RegionMembershipEntity()
            {
                UserEmail = email,
                RegionId = region.Id,
                RegionRoleId = regionRoleId
            });
            await coreDbContext.SaveChangesAsync();

            return result.Entity;
        }
    }
}
