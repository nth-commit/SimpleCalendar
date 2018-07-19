using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCalendar.Api.Core.Data
{
    public static class RegionMembershipQueries
    {
        public static async Task<IEnumerable<RegionMembershipEntity>> GetRegionMembershipsAsync(
            this CoreDbContext coreDbContext,
            string userEmail) =>
                (await coreDbContext.RegionMemberships
                    .Where(rm => rm.UserEmail.Equals(userEmail, StringComparison.OrdinalIgnoreCase))
                    .ToListAsync()).AsEnumerable();
    }
}
