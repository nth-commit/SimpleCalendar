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
            string codes)
        {
            if (codes == Constants.RootRegionId)
            {
                return coreDbContext.GetRegionByIdAsync(Constants.RootRegionId);
            }

            return coreDbContext.GetRegionByCodesAsync(string.IsNullOrEmpty(codes) ?
                Enumerable.Empty<string>() :
                codes.ToLower().Split('/'));
        }

        public static async Task<RegionEntity> GetRegionByCodesAsync(
            this CoreDbContext coreDbContext,
            IEnumerable<string> codes)
        {
            var hierarchy = await GetRegionHierarchyByCodesAsync(coreDbContext, codes);

            if (!codes.Any())
            {
                return hierarchy.Single();
            }

            var candidate = hierarchy.LastOrDefault();
            return candidate.Code == codes.LastOrDefault() ? candidate : null;
        }

        private static async Task<IEnumerable<RegionEntity>> GetRegionHierarchyByCodesAsync(
            this CoreDbContext coreDbContext,
            IEnumerable<string> codes)
        {
            if (!codes.Any())
            {
                var rootRegion = await coreDbContext.Regions
                    .Include(r => r.Memberships)
                    .Where(r => r.Id == Constants.RootRegionId)
                    .FirstOrDefaultAsync();

                return new List<RegionEntity>() { rootRegion };
            }

            var ancestors = await GetRegionHierarchyByCodesAsync(coreDbContext, codes.Take(codes.Count() - 1));
            var parent = ancestors.Last();

            var nextCode = codes.LastOrDefault();
            if (nextCode == null)
            {
                return ancestors;
            }

            RegionEntity region = null;
            if (parent.Id == Constants.RootRegionId)
            {
                region = await coreDbContext.Regions
                    .Include(r => r.Children)
                    .Include(r => r.Memberships)
                    .FirstOrDefaultAsync(r =>
                        r.ParentId == Constants.RootRegionId &&
                        r.Code == nextCode);
            }
            else
            {
                var id = parent.Children.SingleOrDefault(r => r.Code == nextCode)?.Id;
                if (!string.IsNullOrEmpty(id))
                {
                    region = await coreDbContext.Regions
                        .Include(r => r.Children)
                        .Include(r => r.Memberships)
                        .SingleAsync(r => r.Id == id);
                }
            }

            if (region == null)
            {
                return ancestors;
            }

            return ancestors.Concat(new List<RegionEntity>() { region });
        }

        public static async Task<RegionEntity> GetRegionByCodesAsync2(
            this CoreDbContext coreDbContext,
            IEnumerable<string> codes)
        {
            var query = coreDbContext.Regions
                .Include(r => r.Memberships)
                .Where(r => r.Id == Constants.RootRegionId);

            foreach (var code in codes)
            {
                query = query
                    .Join(
                        coreDbContext.Regions,
                        a => a.Id,
                        b => b.ParentId,
                        (a, b) => b)
                    .Include(r => r.Memberships)
                    .Include(r => r.Parent).ThenInclude(r => r.Memberships)
                    .Include(r => r.Parent).ThenInclude(r => r.Parent).ThenInclude(r => r.Memberships)
                    .Where(r => r.Code == code);
            }

            var region = await query.Include(r => r.Children).FirstOrDefaultAsync();

            var level1Region = region.GetRegions().First();
            level1Region.Parent = await coreDbContext.Regions.FindAsync(Constants.RootRegionId);

            return region;
        }

        // TODO: Make this query way faster
        public static async Task<RegionEntity> GetRegionByIdAsync(
            this CoreDbContext coreDbContext,
            string id,
            bool includeChildren = true)
        {
            var regionQuery = coreDbContext.Regions
                .Where(r => r.Id == id)
                .Include(r => r.Memberships)
                .AsQueryable();

            if (includeChildren)
            {
                regionQuery = regionQuery.Include(r => r.Children);
            }

            var region = await regionQuery.SingleOrDefaultAsync();

            if (!string.IsNullOrEmpty(region.ParentId))
            {
                region.Parent = await coreDbContext.GetRegionByIdAsync(region.ParentId, includeChildren: false);
            }

            return region;
        }
    }
}
