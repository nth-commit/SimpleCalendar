using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleCalendar.Api.UnitTests
{
    public static class GivenAnyContextUserExtensions
    {
        public static void SetupUserIsRootAdministrator(this GivenAnyContext context)
            => context.SetupUser("google-oauth2|103074202427969604113");

        public static void SetupUser(this GivenAnyContext context, string userId)
            => context.UserId.Setup(x => x.Value).Returns(userId);
    }
}
