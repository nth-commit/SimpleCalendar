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
            await _coreDbContext.Database.EnsureCreatedAsync();

            var regions = new List<RegionCreate>()
            {
                new RegionCreate()
                {
                    Name = "New Zealand"
                },
                new RegionCreate()
                {
                    Name = "Wellington",
                    ParentId = "new-zealand"
                }
            };

            var regionsToUpdate = new List<RegionCreate>();
            foreach (var region in regions)
            {
                var regionCreateResult = await _regionService.CreateRegionAsync(region);
                switch (regionCreateResult.Status)
                {
                    case RegionCreateResult.RegionCreateResultStatus.Success:
                        break;
                    case RegionCreateResult.RegionCreateResultStatus.RegionAlreadyExists:
                        regionsToUpdate.Add(region);
                        break;
                    default:
                        throw new Exception($"Failed to create region: {regionCreateResult.Status}");
                }
            }

            // TODO: Update regions
        }
    }
}
