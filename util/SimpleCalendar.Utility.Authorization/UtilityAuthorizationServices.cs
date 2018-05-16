using Microsoft.AspNetCore.Authorization;
using SimpleCalendar.Framework.Identity;
using SimpleCalendar.Utility.Authorization;
using SimpleCalendar.Utility.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class UtilityAuthorizationServices
    {
        public static TServiceCollection AddAuthorizationUtilityServices<TServiceCollection>(
            this TServiceCollection services)
            where TServiceCollection : IValidatableServiceCollection
        {
            services.AddTransient<IClaimsPrincipalAuthorizationService, DefaultClaimsPrincipalAuthorizationService>();

            services.AddRequirement<IClaimsPrincipalAccessor>();
            services.AddRequirement<IAuthorizationService>();

            return services;
        }
    }
}
