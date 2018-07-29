using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using SimpleCalendar.Api.Core.Data;
using SimpleCalendar.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleCalendar.Api.Services
{
    public interface IRegionCache
    {
        Task<RegionEntity> GetRegionAsync(string regionId);

        Task<IEnumerable<RegionEntity>> GetRegionsByParentAsync(string parentRegionId);

        Task RefreshAsync();
    }

    public class RegionCache : IRegionCache
    {
        private static readonly string CacheKey = nameof(RegionCache);

        private readonly IMemoryCache _memoryCache;
        private readonly CoreDbContext _coreDbContext;

        public RegionCache(
            CoreDbContext coreDbContext,
            IMemoryCache memoryCache)
        {
            _coreDbContext = coreDbContext;
            _memoryCache = memoryCache;
        }

        public async Task<RegionEntity> GetRegionAsync(string regionId)
        {
            var regionLookups = await GetRegionsFromCacheAsync();

            IEnumerable<RegionEntity> regionHierarchy;
            try
            {
                regionHierarchy = GetRegionHierarchy(regionLookups.RegionsById, regionId).ToList();
            }
            catch (KeyNotFoundException)
            {
                return null;
            }

            return PrepareRegion(regionHierarchy);
        }

        public async Task<IEnumerable<RegionEntity>> GetRegionsByParentAsync(string parentRegionId)
        {
            var regionLookups = await GetRegionsFromCacheAsync();
            return regionLookups.RegionsByParentId[parentRegionId]
                .Select(r => GetRegionHierarchy(regionLookups.RegionsById, r.Id))
                .Select(r => PrepareRegion(r));
        }

        public Task RefreshAsync()
        {
            _memoryCache.Remove(CacheKey);
            return Task.CompletedTask;
        }

        private Task<RegionLookups> GetRegionsFromCacheAsync() =>
            _memoryCache.GetOrCreateAsync(CacheKey, async (entry) =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);

                var regions = await _coreDbContext.Regions.AsNoTracking().ToListAsync();
                regions.ForEach(r =>
                {
                    r.Parent = null;
                    r.Children = null;
                });

                return new RegionLookups()
                {
                    RegionsById = regions.ToDictionary(r => r.Id),
                    RegionsByParentId = regions.ToLookup(r => r.ParentId)
                };
            });

        private IEnumerable<RegionEntity> GetRegionHierarchy(Dictionary<string, RegionEntity> regionsById, string regionId)
        {
            while (!string.IsNullOrEmpty(regionId))
            {
                var region = regionsById[regionId];
                regionId = region.ParentId;
                yield return region;
            }
        }

        private RegionEntity PrepareRegion(IEnumerable<RegionEntity> regionHierarchy) =>
            regionHierarchy
                .Reverse()
                .Select(r => Copy(r))
                .Aggregate((prev, curr) =>
                {
                    if (prev == null)
                    {
                        return curr;
                    }

                    curr.Parent = prev;
                    return curr;
                });

        private RegionEntity Copy(RegionEntity region) =>
            JsonConvert.DeserializeObject<RegionEntity>(JsonConvert.SerializeObject(region));

        private class RegionLookups
        {
            public Dictionary<string, RegionEntity> RegionsById { get; set; }

            public ILookup<string, RegionEntity> RegionsByParentId { get; set; }
        }
    }
}
