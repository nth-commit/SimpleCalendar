using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using SimpleCalendar.Api.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCalendar.Api.Core.Authorization
{
    public interface IRegionRoleCache
    {
        Task<IEnumerable<RegionRoleEntity>> ListAsync();
    }

    public class RegionRoleCache : IRegionRoleCache
    {
        private static readonly object __cacheRef = new object();

        private readonly IMemoryCache _memoryCache;
        private readonly CoreDbContext _coreDbContext;

        public RegionRoleCache(
            IMemoryCache memoryCache,
            CoreDbContext coreDbContext)
        {
            _memoryCache = memoryCache;
            _coreDbContext = coreDbContext;
        }

        public async Task<IEnumerable<RegionRoleEntity>> ListAsync() => (await _coreDbContext.RegionRoles.ToListAsync()).AsEnumerable();
    }
}
