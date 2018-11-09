using System.Net.Http;
using System.Threading.Tasks;

namespace SimpleCalendar.Api.Test.Data
{
    public interface IHttpClientTestDataInitializer
    {
        Task Initialize(HttpClient client);
    }
}
