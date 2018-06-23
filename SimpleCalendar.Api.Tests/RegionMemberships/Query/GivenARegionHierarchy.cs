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
        public GivenARegionHierarchy() => InitalizeAsync().GetAwaiter().GetResult();

        private async Task InitalizeAsync()
        {
            await this.GivenARegionHierarchyAsync();
        }

        protected Task QueryMembershipsAndAssertUnauthorizedAsync() => QueryMembershipsAndAssertStatusAsync(HttpStatusCode.Unauthorized);

        protected Task QueryMembershipsAndAssertOKAsync() => QueryMembershipsAndAssertStatusAsync(HttpStatusCode.OK);

        protected async Task QueryMembershipsAndAssertStatusAsync(
            HttpStatusCode httpStatusCode)
        {
            var query = new Dictionary<string, string>();

            var path = "/regionmemberships";
            if (query.Any())
            {
                path += "?" + string.Join("&", query.Select(kvp => $"{kvp.Key}={kvp.Value}"));
            }

            var response = await Client.GetAsync(path);
            response.AssertStatusCode(httpStatusCode);
        }
    }
}
