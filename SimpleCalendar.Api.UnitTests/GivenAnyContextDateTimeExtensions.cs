using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleCalendar.Api.UnitTests
{
    public static class GivenAnyContextDateTimeExtensions
    {
        public static void GivenTheCurrentUtcDateTime(
            this GivenAnyContext context,
            DateTime utcNow)
        {
            context.DateTimeAccessor.Setup(x => x.UtcNow).Returns(utcNow);
        }
    }
}
