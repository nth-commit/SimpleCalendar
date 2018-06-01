using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCalendar.Api.Core.Data
{
    public static class EventQueries
    {
        public static async Task<EventEntity> GetEventByIdAsync(
            this CoreDbContext coreDbContext,
            string id)
        {
            var query = coreDbContext.Events
                .Where(e => e.Id == id)
                .Include(e => e.Region);

            for (var i = 0; i < 10; i++)
            {
                query = query.ThenInclude(r => r.Parent);
            }

            return await query.FirstOrDefaultAsync();
        }
    }
}
