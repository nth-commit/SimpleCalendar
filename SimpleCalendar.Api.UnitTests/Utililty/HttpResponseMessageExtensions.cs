using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace System.Net.Http
{
    public static class EventsHttpResponseMessageExtensions
    {
        public static void AssertStatusCode(this HttpResponseMessage response, HttpStatusCode expectedStatusCode)
            => Assert.Equal(expectedStatusCode, response.StatusCode);

        public static void AssertStatusCodeOK(this HttpResponseMessage response) => response.AssertStatusCode(HttpStatusCode.OK);

        public static void AssertStatusCodeBadRequest(this HttpResponseMessage response) => response.AssertStatusCode(HttpStatusCode.BadRequest);
    }
}
