using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleCalendar.Api.UnitTests
{
    public static class GivenAnyContextUserExtensions
    {
        public static void SetupUserIsRootAdministrator(this GivenAnyContext context)
            => context.UserId.Setup(x => x.Value).Returns("google-oauth2|103074202427969604113");
    }
}
