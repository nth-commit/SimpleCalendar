using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCalendar.Api.Core.Data
{
    public static class RegionQueries
    {
        public static Task<RegionEntity> GetRegionByCodesAsync(
            this CoreDbContext coreDbContext,
            string codesJoined)
                => coreDbContext.GetRegionByCodesAsync(codesJoined.ToLower().Split('.'));

        public static async Task<RegionEntity> GetRegionByCodesAsync(
            this CoreDbContext coreDbContext,
            IEnumerable<string> codes)
        {
            var query = coreDbContext.Regions
                .Include(r => r.Roles)
                .Where(r => r.Id == Constants.RootRegionId);

            // Force the root region to be loaded.
            var x = await query.FirstOrDefaultAsync();

            foreach (var code in codes)
            {
                query = query
                    .Join(
                        coreDbContext.Regions,
                        a => a.Id,
                        b => b.ParentId,
                        (a, b) => b)
                    .Include(r => r.Roles)
                    .Include(r => r.Parent).ThenInclude(r => r.Roles)
                    .Include(r => r.Parent).ThenInclude(r => r.Parent).ThenInclude(r => r.Roles)
                    .Where(r => r.Code == code);
            }

            return await query
                .Include(r => r.Children)
                .FirstOrDefaultAsync();
        }

        // TODO: Make this query way faster
        public static async Task<RegionEntity> GetRegionByIdAsync(
            this CoreDbContext coreDbContext,
            string id)
        {
            var region = await coreDbContext.Regions
                .Where(r => r.Id == id)
                .Include(r => r.Roles)
                .SingleOrDefaultAsync();

            if (!string.IsNullOrEmpty(region.ParentId))
            {
                region.Parent = await coreDbContext.GetRegionByIdAsync(region.ParentId);
            }

            return region;
        }
    }
}
