using SimpleCalendar.Api.Core.Regions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCalendar.Api.UnitTests
{
    public static class GivenAnyContextRegionExtensions
    {
        private readonly static IReadOnlyList<RegionCreate> _regions = new List<RegionCreate>()
        {
            new RegionCreate()
            {
                Name = "New Zealand"
            },
            new RegionCreate()
            {
                Name = "Wellington",
                ParentId = "new_zealand"
            },
            new RegionCreate()
            {
                Name = "Mount Victoria",
                ParentId = "new_zealand.wellington"
            },
            new RegionCreate()
            {
                Name = "Auckland",
                ParentId = "new_zealand"
            }
        };

        public static async Task CreateRegionHierarchyAsync(this GivenAnyContext context)
        {
            var regionService = context.GetRequiredService<RegionService>();
            foreach (var region in _regions)
            {
                await regionService.CreateRegionAsync(region);
            }
        }
    }
}
