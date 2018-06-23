using Newtonsoft.Json;
using SimpleCalendar.Api.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SimpleCalendar.Api.UnitTests.RegionMemberships.Create
{
    public class GivenAValidRegionMembership : GivenAnyContext
    {
        protected const string Level1RegionId = GivenAnyContextRegionExtensions.Level1RegionId;
        protected const string Level2RegionId = GivenAnyContextRegionExtensions.Level2RegionId;
        protected const string Level3RegionId = GivenAnyContextRegionExtensions.Level3RegionId;

        protected const string Level1AdministratorId = "Level1Administrator";
        protected const string Level2AdministratorId = "Level2Administrator";
        protected const string Level3AdministratorId = "Level3Administrator";

        protected RegionMembershipCreate ValidRegionMembership => ValidRegionMembershipLevel1;

        protected RegionMembershipCreate ValidRegionMembershipLevel1 => new RegionMembershipCreate()
        {
            UserId = "user@example.com",
            RegionId = Level1RegionId,
            Role = RegionMembershipRole.User
        };

        protected RegionMembershipCreate ValidRegionMembershipLevel2 => new RegionMembershipCreate()
        {
            UserId = "user@example.com",
            RegionId = Level2RegionId,
            Role = RegionMembershipRole.User
        };

        protected RegionMembershipCreate ValidRegionMembershipLevel3 => new RegionMembershipCreate()
        {
            UserId = "user@example.com",
            RegionId = Level3RegionId,
            Role = RegionMembershipRole.User
        };

        public GivenAValidRegionMembership() => InitializeAsync().GetAwaiter().GetResult();

        private async Task InitializeAsync()
        {
            await this.GivenARegionHierarchyAsync();
        }

        protected Task<HttpResponseMessage> GetCreateHttpResponseMessageAsync(RegionMembershipCreate create)
            => Client.CreateRegionMembershipAsync(create);

        protected async Task<RegionMembership> CreateAndAssertCreatedAsync(RegionMembershipCreate create)
        {
            var response = await CreateAndAssertStatusAsync(create, HttpStatusCode.Created);
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<RegionMembership>(json);
        }

        protected async Task<Dictionary<string, IEnumerable<string>>> CreateAndAssertBadRequestAsync(RegionMembershipCreate create)
        {
            var response = await CreateAndAssertStatusAsync(create, HttpStatusCode.BadRequest);
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Dictionary<string, IEnumerable<string>>>(json);
        }

        protected Task CreateAndAssertUnauthorizedAsync(RegionMembershipCreate create)
            => CreateAndAssertStatusAsync(create, HttpStatusCode.Unauthorized);

        protected async Task<HttpResponseMessage> CreateAndAssertStatusAsync(RegionMembershipCreate create, HttpStatusCode httpStatusCode)
        {
            var response = await GetCreateHttpResponseMessageAsync(create);
            response.AssertStatusCode(httpStatusCode);
            return response;
        }

        public class Tests : GivenAValidRegionMembership
        {
            public Tests()
            {
                this.GivenIAmARootAdministrator();
            }

            [Fact]
            public async Task WhenICreateTheMembership_ItReturns201Created()
            {
                await CreateAndAssertCreatedAsync(ValidRegionMembership);
            }
        }
    }
}
