using System.Net.Http;
using System.Threading.Tasks;
using SimpleCalendar.Api.Core.Data;

namespace SimpleCalendar.Api.Test.Data.Initializers
{
    public class RegionInitializer : IDbContextTestDataInitializer
    {
        public Task Initialize(CoreDbContext coreDbContext) => coreDbContext
            .AddRegionAsync("new-zealand", "Dunedin")
            .AddRegionAsync("new-zealand", "Christchurch")
            .SaveChangesAsync();
    }
}
