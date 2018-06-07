using Newtonsoft.Json;
using SimpleCalendar.Api.Core.Regions;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCalendar.Api.UnitTests.Regions
{
    public static class ResponseHttpMessageExtensions
    {
        public static async Task<IEnumerable<RegionResult>> DeserializeRegionsAsync(
            this HttpResponseMessage response)
        {
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<RegionResult>>(json);
        }
    }
}
