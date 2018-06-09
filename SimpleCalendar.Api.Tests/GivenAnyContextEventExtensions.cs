using SimpleCalendar.Api.Core.Data;
using SimpleCalendar.Api.Core.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCalendar.Api.UnitTests
{
    public static class GivenAnyContextEventExtensions
    {
        public static async Task GivenAnEventAsync(
            this GivenAnyContext context,
            EventEntity ev)
        {
            var dbContext = context.GetCoreDbContext();

            await dbContext.Events.AddAsync(ev);
            await dbContext.SaveChangesAsync();
        }
    }
}
