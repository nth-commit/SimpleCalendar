using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace System.Net.Http
{
    public static class HttpResponseMessageExtensions
    {
        public static void AssertStatusCode(this HttpResponseMessage response, HttpStatusCode expectedStatusCode)
            => Assert.Equal(expectedStatusCode, response.StatusCode);
    }
}
