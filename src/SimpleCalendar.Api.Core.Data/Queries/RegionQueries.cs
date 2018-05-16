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
        public static async Task<RegionEntity> GetRegionByCodesAsync(
            this CoreDbContext coreDbContext,
            IEnumerable<string> codes)
        {
            var query = coreDbContext.Regions
                .Include(r => r.Parent)
                .Where(r => r.ParentId == Data.Constants.RootRegionId)
                .Where(r => r.Code == codes.First());

            foreach (var code in codes.Skip(1))
            {
                query = query
                    .Join(
                        coreDbContext.Regions,
                        a => a.Id,
                        b => b.ParentId,
                        (a, b) => b)
                    .Include(r => r.Parent)
                    .Where(r => r.Code == code);
            }

            return await query
                .Include(r => r.Children)
                .FirstOrDefaultAsync();
        }
    }
}
