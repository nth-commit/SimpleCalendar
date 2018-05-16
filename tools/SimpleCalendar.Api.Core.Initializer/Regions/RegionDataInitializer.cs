using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SimpleCalendar.Api.Core.Data;
using SimpleCalendar.Api.Core.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCalendar.Api.Core.Initializer.Regions
{
    public class RegionDataInitializer : IDataInitializer
    {
        private readonly RegionService _regionService;
        private readonly CoreDbContext _coreDbContext;
        private readonly IMapper _mapper;

        public RegionDataInitializer(
            RegionService regionService,
            CoreDbContext coreDbContext,
            IMapper mapper)
        {
            _regionService = regionService;
            _coreDbContext = coreDbContext;
            _mapper = mapper;
        }

        public async Task RunAsync()
        {
            var regions = new RegionCreate[]
            {
                new RegionCreate()
                {
                    Id = "266fde3e-18da-44b4-9880-d7a3235a4f0f",
                    Name = "New Zealand"
                },
                new RegionCreate()
                {
                    Id = "092460d3-d85e-4df8-81b6-07a38c105307",
                    Name = "Wellington",
                    ParentId = "266fde3e-18da-44b4-9880-d7a3235a4f0f"
                }
            };

            var regionsById = regions.ToDictionary(r => r.Id);
            var regionIds = regionsById.Keys;

            var existingRegions = await _coreDbContext.Regions.ToListAsync();
            var existingRegionsById = existingRegions.ToDictionary(r => r.Id);
            var existingRegionIds = existingRegionsById.Keys;

            var regionIdsToAdd = regionIds.Except(existingRegionIds);
            var regionIdsToUpdate = regionIds.Intersect(existingRegionIds);

            foreach (var regionId in regionIdsToAdd)
            {
                await _regionService.CreateRegionAsync(regionsById[regionId]);
            }

            foreach (var regionId in regionIdsToUpdate)
            {
                var region = regionsById[regionId];
                var existingRegion = existingRegionsById[regionId];
                if (existingRegion.ParentId != region.ParentId)
                {
                    throw new Exception("Cannot update parent region");
                }

                var regionUpdate = _mapper.Map<RegionUpdate>(region);
                await _regionService.UpdateRegionAsync(region.Id, regionUpdate);
            }
        }
    }
}
