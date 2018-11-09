using SimpleCalendar.Api.Core.Data;
using System.Net.Http;
using System.Threading.Tasks;

namespace SimpleCalendar.Api.Test.Data.Initializers
{
    public class RegionMembershipInitializer : IHttpClientTestDataInitializer
    {
        public Task Initialize(HttpClient client) => client
            .CreateRegionMembership("wellingtonveganactions@gmail.com", "new-zealand/wellington", Constants.RegionRoles.Administrator);
    }
}
