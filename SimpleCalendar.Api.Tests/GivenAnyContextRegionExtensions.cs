using SimpleCalendar.Api.Core.Regions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCalendar.Api.UnitTests
{
    public static class GivenAnyContextRegionExtensions
    {
        public const string Level1RegionId = "new_zealand";
        public const string Level2RegionId = "new_zealand.wellington";
        public const string Level3RegionId = "new_zealand.wellington.mount_victoria";

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
    }
}
