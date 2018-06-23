using Microsoft.EntityFrameworkCore;
using SimpleCalendar.Framework.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCalendar.Api.Core.Data
{
    public static class RegionRoleQueries
    {
        public static Task<bool> IsAnyAdministratorAsync(this CoreDbContext dbContext, string userId)
            => dbContext.IsInAnyRoleAsync(userId, Role.Administrator);


        public static Task<bool> IsAnyUserAsync(this CoreDbContext dbContext, string userId)
            => dbContext.IsInAnyRoleAsync(userId, Role.User);

        private static Task<bool> IsInAnyRoleAsync(this CoreDbContext dbContext, string userId, Role role)
            => dbContext.RegionRoles
                .Where(r => r.UserId == userId)
                .Where(r => r.Role.HasFlag(role))
                .AnyAsync();
    }
}
