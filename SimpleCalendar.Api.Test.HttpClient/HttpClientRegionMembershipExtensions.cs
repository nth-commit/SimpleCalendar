using Newtonsoft.Json;
using System.Text;
using System.Threading.Tasks;

namespace System.Net.Http
{
    public static class HttpClientRegionMembershipExtensions
    {
        public static async Task<HttpClient> CreateRegionMembership(
            this HttpClient client, string userEmail, string regionId, string regionRoleId)
        {
            var json = JsonConvert.SerializeObject(new { userEmail, regionId, regionRoleId });
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("regionmemberships", content);
            response.EnsureSuccessStatusCode();

            return client;
        }

        public static async Task<HttpClient> CreateRegionMembership(
            this Task<HttpClient> clientTask, string userEmail, string regionId, string regionRoleId)
        {
            var client = await clientTask;
            return await client.CreateRegionMembership(userEmail, regionId, regionRoleId);
        }
    }
}
