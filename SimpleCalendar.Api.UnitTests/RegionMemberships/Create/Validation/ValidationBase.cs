using SimpleCalendar.Api.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SimpleCalendar.Api.UnitTests.RegionMemberships.Create.Validation
{
    public class ValidationBase : GivenAValidRegionMembership
    {
        public ValidationBase()
        {
            this.GivenIAmARootSuperAdministrator();
        }
        public async Task AssertInvalidMembershipAsync(string expectedInvalidProperty, RegionMembershipCreate create)
        {
            var badRequestResult = await CreateAndAssertBadRequestAsync(create);
            Assert.Contains(badRequestResult, kvp => kvp.Key == expectedInvalidProperty);
        }

        public async Task AssertInvalidMembershipAsync(string expectedInvalidProperty, Action<RegionMembershipCreate> modifyValidMembership)
        {
            var invalidMembership = ValidRegionMembership;
            modifyValidMembership(invalidMembership);
            await AssertInvalidMembershipAsync(expectedInvalidProperty, invalidMembership);
        }
    }
}
