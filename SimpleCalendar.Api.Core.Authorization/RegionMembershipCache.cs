using Microsoft.EntityFrameworkCore;
using SimpleCalendar.Api.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCalendar.Api.Core.Authorization
{
    public interface IRegionMembershipCache
    {
        Task<IEnumerable<RegionMembershipEntity>> ListRegionMembershipsAsync(string userEmail);
    }

    public class RegionMembershipCache : IRegionMembershipCache
    {
        private readonly CoreDbContext _coreDbContext;

        public RegionMembershipCache(
            CoreDbContext coreDbContext)
        {
            _coreDbContext = coreDbContext;
        }

        public async Task<IEnumerable<RegionMembershipEntity>> ListRegionMembershipsAsync(string userEmail)
        {
            if (string.IsNullOrEmpty(userEmail))
            {
                throw new ArgumentNullException(nameof(userEmail));
            }

            return await _coreDbContext.RegionMemberships
                .Where(rm => rm.UserEmail.Equals(userEmail, StringComparison.InvariantCultureIgnoreCase))
                .ToListAsync();
        }
    }
}
