using Newtonsoft.Json;
using SimpleCalendar.Api.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCalendar.Api.UnitTests.Regions
{
    public static class ResponseHttpMessageExtensions
    {
        public static async Task<IEnumerable<Region>> DeserializeRegionsAsync(
            this HttpResponseMessage response)
        {
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<Region>>(json);
        }

        public static async Task<Region> DeserializeRegionAsync(
            this HttpResponseMessage response)
        {
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Region>(json);
        }
    }
}
