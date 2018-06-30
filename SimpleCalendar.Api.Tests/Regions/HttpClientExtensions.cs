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
            this HttpClient httpClient,
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

            var response = await httpClient.GetAsync(path);
            response.AssertStatusCodeOK();

            return await response.DeserializeRegionsAsync();
        }
    }
}
