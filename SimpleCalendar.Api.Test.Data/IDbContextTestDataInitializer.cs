using SimpleCalendar.Api.Core.Data;
using System.Threading.Tasks;

namespace SimpleCalendar.Api.Test.Data
{
    public interface IDbContextTestDataInitializer
    {
        Task Initialize(CoreDbContext coreDbContext);
    }
}
