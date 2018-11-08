using System.Threading.Tasks;

namespace SimpleCalendar.Api.Core.Data
{
    public static class CoreDbContextExtensions
    {
        public static async Task SaveChangesAsync(this Task<CoreDbContext> coreDbContextTask)
        {
            var coreDbContext = await coreDbContextTask;
            await coreDbContext.SaveChangesAsync();
        }
    }
}
