using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace SimpleCalendar.Api.UnitTests.Utility
{
    public static class CollectionAssert
    {
        public static void SetEqual<T>(
            IEnumerable<T> expected,
            IEnumerable<T> actual)
        {
            var notInExpected = expected.Except(actual);
            if (notInExpected.Any())
            {
                Assert.True(false, $"The following values were found but not expected: {string.Join(", ", notInExpected)}");
            }

            var notInActual = actual.Except(expected);
            if (notInActual.Any())
            {
                Assert.True(false, $"The following values were found but not expected: {string.Join(", ", notInActual)}");
            }
        }
    }
}
