using SimpleCalendar.Api.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SimpleCalendar.Api.UnitTests.RegionMemberships.Delete
{
    public class GivenADataStoreWithExistingRegionMemberships : GivenAnyContext
    {
        protected const string Level1RegionId = GivenAnyContextRegionExtensions.Level1RegionId;
        protected const string Level2RegionId = GivenAnyContextRegionExtensions.Level2RegionId;
        protected const string Level3RegionId = GivenAnyContextRegionExtensions.Level3RegionId;

        protected string ExistingMembershipId => ExistingLevel2UserMembershipId;

        protected string ExistingLevel1UserMembershipId { get; private set; }

        protected string ExistingLevel2UserMembershipId { get; private set; }

        protected string ExistingLevel3UserMembershipId { get; private set; }

        protected string ExistingLevel1AdministratorMembershipId { get; private set; }

        protected string ExistingLevel2AdministratorMembershipId { get; private set; }

        protected string ExistingLevel3AdministratorMembershipId { get; private set; }


        public GivenADataStoreWithExistingRegionMemberships() => InitializeAsync().GetAwaiter().GetResult();

        private async Task InitializeAsync()
        {
            await this.GivenARegionHierarchyAsync();

            ExistingLevel1UserMembershipId = await CreateMembershipAsync(Level1RegionId, RegionMembershipRole.User);
            ExistingLevel2UserMembershipId = await CreateMembershipAsync(Level2RegionId, RegionMembershipRole.User);
            ExistingLevel3UserMembershipId = await CreateMembershipAsync(Level3RegionId, RegionMembershipRole.User);

            ExistingLevel1AdministratorMembershipId = await CreateMembershipAsync(Level1RegionId, RegionMembershipRole.Administrator);
            ExistingLevel2AdministratorMembershipId = await CreateMembershipAsync(Level2RegionId, RegionMembershipRole.Administrator);
            ExistingLevel3AdministratorMembershipId = await CreateMembershipAsync(Level3RegionId, RegionMembershipRole.Administrator);
        }

        private async Task<string> CreateMembershipAsync(string regionId, RegionMembershipRole role)
        {
            return (await this.GivenARegionRoleAsync($"{regionId}_{role}", regionId, role)).Id;
        }

        private string GetUserMembershipId(int regionLevel)
        {
            switch (regionLevel)
            {
                case 1:
                    return ExistingLevel1UserMembershipId;
                case 2:
                    return ExistingLevel2UserMembershipId;
                case 3:
                    return ExistingLevel3UserMembershipId;
                default:
                    throw new Exception("Membership id not found");
            }
        }

        private string GetAdministratorMembershipId(int regionLevel)
        {
            switch (regionLevel)
            {
                case 1:
                    return ExistingLevel1AdministratorMembershipId;
                case 2:
                    return ExistingLevel2AdministratorMembershipId;
                case 3:
                    return ExistingLevel3AdministratorMembershipId;
                default:
                    throw new Exception("Membership id not found");
            }
        }

        protected Task DeleteUserAndAssertUnauthorizedAsync(int regionLevel)
            => DeleteAndAssertUnauthorizedAsync(GetUserMembershipId(regionLevel));

        protected Task DeleteAdministratorAndAssertUnauthorizedAsync(int regionLevel)
            => DeleteAndAssertUnauthorizedAsync(GetAdministratorMembershipId(regionLevel));

        protected Task DeleteAndAssertUnauthorizedAsync(string id) => DeleteAndAssertStatusAsync(id, HttpStatusCode.Unauthorized);

        protected Task DeleteAndAssertNoContentAsync(string id) => DeleteAndAssertStatusAsync(id, HttpStatusCode.NoContent);

        protected async Task DeleteAndAssertStatusAsync(string id, HttpStatusCode httpStatusCode)
        {
            var response = await Client.DeleteAsync($"/regionmemberships/{id}");
            response.AssertStatusCode(httpStatusCode);
        }

        public class Tests : GivenADataStoreWithExistingRegionMemberships
        {
            [Fact]
            public async Task GivenIAmARootAdministrator_WhenIDeleteRegionRole_ItReturns204NoContent()
            {
                this.GivenIAmARootAdministrator();
                await DeleteAndAssertNoContentAsync(ExistingMembershipId);
            }

            [Fact]
            public async Task GivenIAmALevel1Administrator_WhenIDeleteRegionRole_ItReturns204NoContent()
            {
                await this.GivenIAmARegionAdministratorAsync("Level1Admin", Level1RegionId);
                await DeleteAndAssertNoContentAsync(ExistingMembershipId);
            }

            [Fact]
            public async Task GivenIAmALevel2Administrator_WhenIDeleteRegionRole_ItReturns403Unauthorized()
            {
                await this.GivenIAmARegionAdministratorAsync("Level2Admin", Level2RegionId);
                await DeleteAndAssertUnauthorizedAsync(ExistingMembershipId);
            }
        }
    }
}
