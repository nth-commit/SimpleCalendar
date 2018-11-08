using System.Threading.Tasks;

namespace SimpleCalendar.Api.Test.Data
{
    public interface ITestDataInitializer<TStartup> where TStartup : class
    {
        Task Initialize();
    }
}
