using Newtonsoft.Json;
using SimpleCalendar.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCalendar.Api.UnitTests.RegionMemberships.Query
{
    public class GivenARegionHierarchy : GivenAnyContext
    {
        protected const string Level1RegionId = GivenAnyContextRegionExtensions.Level1RegionId;
        protected const string Level2RegionId = GivenAnyContextRegionExtensions.Level2RegionId;
        protected const string Level2ARegionId = GivenAnyContextRegionExtensions.Level2ARegionId;
        protected const string Level2BRegionId = GivenAnyContextRegionExtensions.Level2BRegionId;
        protected const string Level3RegionId = GivenAnyContextRegionExtensions.Level3RegionId;

        public GivenARegionHierarchy() => InitalizeAsync().GetAwaiter().GetResult();

        private async Task InitalizeAsync()
        {
            await this.GivenARegionHierarchyAsync();
        }

        protected Task QueryMembershipsAndAssertUnauthorizedAsync() => QueryMembershipsAndAssertStatusAsync(HttpStatusCode.Unauthorized);

        protected async Task<IEnumerable<RegionMembership>> QueryMembershipsAndAssertOKAsync(
            string regionId = null,
            string userId = null)
        {
            var response = await QueryMembershipsAndAssertStatusAsync(HttpStatusCode.OK, regionId: regionId, userId: userId);
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<RegionMembership>>(json);
        }

        protected async Task<HttpResponseMessage> QueryMembershipsAndAssertStatusAsync(
            HttpStatusCode httpStatusCode,
            string regionId = null,
            string userId = null)
        {
            var query = new Dictionary<string, string>();

            if (regionId != null)
            {
                query.Add(nameof(regionId), regionId);
            }

            if (userId != null)
            {
                query.Add(nameof(userId), userId);
            }

            return await QueryAndAssertStatusAsync("/regionmemberships", query, httpStatusCode);
        }

        protected async Task<IEnumerable<RegionMembership>> QueryMyMembershipsAndAssertOKAsync()
        {
            var response = await QueryMyMembershipsAndAssertStatusAsync(HttpStatusCode.OK);
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<RegionMembership>>(json);
        }

        protected async Task<HttpResponseMessage> QueryMyMembershipsAndAssertStatusAsync(
            HttpStatusCode httpStatusCode)
        {
            var query = new Dictionary<string, string>();

            return await QueryAndAssertStatusAsync("/regionmemberships/my", query, httpStatusCode);
        }

        private async Task<HttpResponseMessage> QueryAndAssertStatusAsync(
            string path,
            Dictionary<string, string> query,
            HttpStatusCode httpStatusCode)
        {
            if (query.Any())
            {
                path += "?" + string.Join("&", query.Select(kvp => $"{kvp.Key}={kvp.Value}"));
            }

            var response = await Client.GetAsync(path);
            response.AssertStatusCode(httpStatusCode);

            return response;
        }
    }
}
