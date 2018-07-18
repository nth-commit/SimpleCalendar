using SimpleCalendar.Api.Core.Regions;
using SimpleCalendar.Api.Core.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

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
                Name = "New Zealand"
            },
            new RegionCreate()
            {
                Name = "Wellington",
                ParentId = Level1RegionId
            },
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
            var regionService = context.GetRequiredService<RegionService>();
            foreach (var region in _regions)
            {
                await regionService.CreateRegionAsync(region);
            }
        }

        public static async Task<RegionEntity> GetAGivenRegionById(this GivenAnyContext context, string id)
        {
            return await context.GetCoreDbContext().GetRegionByCodesAsync(id);
        }
    }
}
