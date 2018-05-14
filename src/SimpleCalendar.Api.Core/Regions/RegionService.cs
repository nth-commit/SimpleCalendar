using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCalendar.Api.Core.Regions
{
    public class RegionService
    {
        private readonly IRegionRepository _regionRepository;

        public RegionService(
            IRegionRepository regionRepository)
        {
            _regionRepository = regionRepository;
        }

        public async Task<Region> CreateRegionAsync(RegionCreate create)
        {
            var idParts = create.Id.Split('.');
            var parentId = string.Join(".", idParts.Take(idParts.Count() - 1));
            var localId = idParts.Last();

            var region = new Region()
            {
                LocalId = localId,
                ParentId = parentId,
                RolesByUser = create.RolesByUser
            };

            var result = await _regionRepository.CreateRegionAsync(region);
            return result;
        }
    }
}
