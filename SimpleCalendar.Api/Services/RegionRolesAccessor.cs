using Microsoft.Extensions.Caching.Memory;
using SimpleCalendar.Api.Core.Data;
using SimpleCalendar.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleCalendar.Api.Services
{
    public class RegionRolesAccessor : IRegionRolesAccessor
    {
        private readonly IMemoryCache _memoryCache;
        private readonly CoreDbContext _coreDbContext;

        public RegionRolesAccessor(
            IMemoryCache memoryCache,
            CoreDbContext coreDbContext)
        {
            _memoryCache = memoryCache;
            _coreDbContext = coreDbContext;
        }

       public IEnumerable<RegionRoleEntity> RegionRoles =>
            _memoryCache.GetOrCreate(nameof(RegionRolesAccessor), entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(60);
                return _coreDbContext.RegionRoles.ToList();
            });
    }
}
