using Microsoft.AspNetCore.Builder;
using SimpleCalendar.Api.Middleware.ExceptionHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Builder
{
    public static class ExceptionHandlerApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionHandlerMiddleware>();
            return app;
        }
    }
}
