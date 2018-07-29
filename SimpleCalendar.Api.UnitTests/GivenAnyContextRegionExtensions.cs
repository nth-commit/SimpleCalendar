using SimpleCalendar.Api.Core.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SimpleCalendar.Api.Models;
using SimpleCalendar.Api.UnitTests.Regions;
using Newtonsoft.Json;

namespace SimpleCalendar.Api.UnitTests
{
    public static class GivenAnyContextRegionExtensions
    {
        public const string Level1RegionId = Constants.Level1RegionId;
        public const string Level2RegionId = Constants.Level2RegionId;
        public const string Level2ARegionId = Constants.Level2ARegionId;
        public const string Level2BRegionId = Constants.Level2BRegionId;
        public const string Level3RegionId = Constants.Level3RegionId;

        private readonly static IReadOnlyList<RegionCreate> _regions = new List<RegionCreate>()
        {
            new RegionCreate()
            {
                Name = "Mount Victoria",
                ParentId = Level2RegionId
            },
            new RegionCreate()
            {
                Name = "Auckland",
                ParentId = Level1RegionId
            }
        };

        public static async Task GivenARegionHierarchyAsync(this GivenAnyContext context)
        {
            var coreDbContext = context.GetCoreDbContext();
            foreach (var region in _regions)
            {
                var id = string.Empty;
                if (region.ParentId != Constants.RootRegionId)
                {
                    id += region.ParentId + "/";
                }
                id += region.Name.ToLower().Replace(' ', '-');

                await coreDbContext.Regions.AddAsync(new RegionEntity()
                {
                    Id = id,
                    ParentId = region.ParentId,
                    DataJson = JsonConvert.SerializeObject(new
                    {
                        region.Name
                    }),
                    DataJsonVersion = 1
                });
            }
            await coreDbContext.SaveChangesAsync();
        }

        public static async Task<RegionEntity> GetAGivenRegionById(this GivenAnyContext context, string id)
        {
            return await context.GetCoreDbContext().Regions.FindAsync(id);
        }
    }
}
