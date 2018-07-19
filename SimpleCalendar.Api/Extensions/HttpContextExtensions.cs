using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Http
{
    public static class HttpContextExtensions
    {
        public static void SetSecurityToken(
            this HttpContext httpContext,
            string securityToken)
        {
            httpContext.Items.Add("SecurityToken", securityToken);
        }

        public static string GetSecurityToken(
            this HttpContext httpContext)
        {
            return (string)httpContext.Items["SecurityToken"];
        }
    }
}
