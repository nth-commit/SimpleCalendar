using Newtonsoft.Json;
using SimpleCalendar.Api.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCalendar.Api.UnitTests.RegionRoles
{
    public static class HttpClientExtensions
    {
        public static async Task<IEnumerable<RegionRole>> GetRegionRolesAndAssertOKAsync(
            this HttpClient client)
        {
            var response = await client.GetAsync("regionroles");
            response.AssertStatusCodeOK();

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<RegionRole>>(json);
        }

        public static async Task GetRegionRolesAndAssertUnauthorizedAsync(
            this HttpClient client)
        {
            var response = await client.GetAsync("regionroles");
            response.AssertStatusCode(HttpStatusCode.Unauthorized);
        }
    }
}
