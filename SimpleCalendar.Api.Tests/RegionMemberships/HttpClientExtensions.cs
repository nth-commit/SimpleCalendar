using Newtonsoft.Json;
using SimpleCalendar.Api.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace System.Net.Http
{
    public static class HttpClientExtensions
    {
        public static async Task<HttpResponseMessage> CreateRegionMembershipAsync(
            this HttpClient client,
            RegionMembershipCreate create)
        {
            var content = new StringContent(JsonConvert.SerializeObject(create), Encoding.UTF8, "application/json");
            return await client.PostAsync("/regionmemberships", content);
        }
    }
}
