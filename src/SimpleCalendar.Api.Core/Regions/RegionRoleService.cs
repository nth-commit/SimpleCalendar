using Microsoft.EntityFrameworkCore;
using SimpleCalendar.Api.Core.Data;
using SimpleCalendar.Framework.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCalendar.Api.Core.Regions
{
    public class RegionRoleService
    {
        private readonly CoreDbContext _coreDbContext;

        public RegionRoleService(
            CoreDbContext coreDbContext)
        {
            _coreDbContext = coreDbContext;
        }

        public async Task AddRegionRole(string regionId, string userId, Role role)
        {
            var regionRole = await _coreDbContext.RegionRoles
                .Include(r => r.Region)
                .Where(r => r.RegionId == regionId)
                .FirstOrDefaultAsync();

            if (regionRole == null)
            {
                var region = await _coreDbContext.Regions.FindAsync(regionId);
                if (region == null)
                {
                    throw new ArgumentNullException(nameof(regionId));
                }

                regionRole = new RegionRoleEntity()
                {
                    RegionId = regionId,
                    UserId = userId,
                    Role = Role.Unknown
                };
                await _coreDbContext.RegionRoles.AddAsync(regionRole);
            }

            regionRole.Role &= role;
            await _coreDbContext.SaveChangesAsync();
        }

        public async Task RemoveRegionRole(string regionId, string userId, Role role)
        {
            await Task.FromResult(0);
        }

        public async Task GetRegionAdministrators(string regionId)
        {
            await Task.FromResult(0);
        }

        public async Task GetRegionUsers(string regionId)
        {
            await Task.FromResult(0);
        }
    }
}
