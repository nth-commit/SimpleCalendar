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
        public static async Task<RegionEntity> GetRegionByIdAsync(
            this CoreDbContext coreDbContext,
            string id,
            bool includeChildren = true)
        {
            var regionQuery = coreDbContext.Regions.Where(r => r.Id == id);

            if (includeChildren)
            {
                regionQuery = regionQuery.Include(r => r.Children);
            }

            var region = await regionQuery.SingleOrDefaultAsync();
            if (region == null)
            {
                return null;
            }

            if (!string.IsNullOrEmpty(region.ParentId))
            {
                region.Parent = await coreDbContext.GetRegionByIdAsync(region.ParentId, includeChildren: false);
            }

            return region;
        }
    }
}
