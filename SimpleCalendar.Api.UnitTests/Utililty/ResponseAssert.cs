using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using Xunit;

namespace SimpleCalendar.Api.UnitTests.Utililty
{
    public static class ResponseAssert
    {
        public static void AssertStatusCode(HttpResponseMessage response, HttpStatusCode expectedStatusCode) =>
            Assert.Equal(expectedStatusCode, response.StatusCode);
    }
}
