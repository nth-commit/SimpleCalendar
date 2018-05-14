using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCalendar.Api.Core.Regions
{
    public interface IRegionRepository
    {
        Task<Region> GetRegionAsync(string id);

        Task<Region> CreateRegionAsync(Region region);
    }

    public static class RegionRepository
    {
        public static Task<Region> GetRootRegionAsync(this IRegionRepository regionRepository) =>
            regionRepository.GetRegionAsync(Constants.RootRegionId);
    }
}
