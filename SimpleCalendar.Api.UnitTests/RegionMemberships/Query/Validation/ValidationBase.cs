using Newtonsoft.Json;
using SimpleCalendar.Api.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SimpleCalendar.Api.UnitTests.RegionMemberships.Query.Validation
{
    public class ValidationBase : GivenARegionHierarchy
    {
        public ValidationBase()
        {
            this.GivenIAmARootSuperAdministrator();
        }

        public async Task AssertInvalidQueryAsync(
            string expectedInvalidProperty,
            string regionId = null,
            string userId = null)
        {
            var response = await QueryMembershipsAndAssertStatusAsync(
                HttpStatusCode.BadRequest,
                regionId: regionId,
                userId: userId);

            var json = await response.Content.ReadAsStringAsync();
            var badRequestResult = JsonConvert.DeserializeObject<Dictionary<string, IEnumerable<string>>>(json);

            Assert.Contains(badRequestResult, kvp => kvp.Key == expectedInvalidProperty);
        }
    }
}
