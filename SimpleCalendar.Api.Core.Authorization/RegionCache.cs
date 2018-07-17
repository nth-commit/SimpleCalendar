using SimpleCalendar.Api.Core.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCalendar.Api.Core.Authorization
{
    public interface IRegionCache
    {
        Task<RegionEntity> GetRegionByCodesAsync(string codesJoined);
    }

    public class RegionCache : IRegionCache
    {
        private readonly CoreDbContext _coreDbContext;

        public RegionCache(
            CoreDbContext coreDbContext)
        {
            _coreDbContext = coreDbContext;
        }

        public async Task<RegionEntity> GetRegionByCodesAsync(string codesJoined)
        {
            return await _coreDbContext.GetRegionByCodesAsync(codesJoined);
        }
    }
}
