using SimpleCalendar.Api.Core.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCalendar.Api.UnitTests.Regions
{
    public static class HttpClientExtensions
    {
        public static async Task<IEnumerable<RegionResult>> GetRegionsAndAssertOKAsync(
            this HttpClient client,
            string parentId = null)
        {
            var queryParams = new Dictionary<string, string>();

            if (!string.IsNullOrEmpty(parentId))
            {
                queryParams.Add(nameof(parentId), parentId);
            }

            var path = "/regions";
            if (queryParams.Any())
            {
                path += "?" + string.Join("&", queryParams.Select(kvp => $"{kvp.Key}={kvp.Value}"));
            }

            var response = await client.GetAsync(path);
            response.AssertStatusCodeOK();

            return await response.DeserializeRegionsAsync();
        }

        public static async Task<RegionResult> GetRegionAndAssertOKAsync(
            this HttpClient client,
            string id)
        {
            var response = await client.GetAsync($"/regions/{id}");
            response.AssertStatusCodeOK();

            return await response.DeserializeRegionAsync();
        }
    }
}
