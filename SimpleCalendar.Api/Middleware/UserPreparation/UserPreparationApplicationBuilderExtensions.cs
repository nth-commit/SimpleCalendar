
using Microsoft.AspNetCore.Builder;
using SimpleCalendar.Api.Middleware.ExceptionHandler;
using SimpleCalendar.Api.Middleware.UserPreparation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Builder
{
    public static class UserPreparationApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseUserPreparation(this IApplicationBuilder app)
        {
            app.UseMiddleware<UserPreparationMiddleware>();
            return app;
        }
    }
}
