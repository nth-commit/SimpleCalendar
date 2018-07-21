using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using SimpleCalendar.Api.Core.Authorization;
using SimpleCalendar.Api.Core.Data;
using SimpleCalendar.Framework;
using SimpleCalendar.Utility.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleCalendar.Utility.DependencyInjection
{
    public static class ApiCoreAuthorizationServices
    {
        public static IValidatableServiceCollection AddApiCoreAuthorizationServices(
            this IValidatableServiceCollection services)
        {
            services.AddTransient<IAuthorizationHandler, RegionPermissionAuthorizationHandler>();
            services.AddTransient<IAuthorizationHandler, EventPermissionAuthorizationHandler>();
            services.AddTransient<IAuthorizationHandler, RegionOperationAuthorizationHandler>();
            services.AddTransient<IAuthorizationHandler, RegionRoleAuthorizationHandler>();
            services.AddTransient<IRegionPermissionResolver, RegionPermissionResolver>();
            services.AddTransient<IEventPermissionResolver, EventPermissionResolver>();

            services.AddRequirement<IAuthorizationService>();
            services.AddRequirement<IRegionRolesAccessor>();

            return services;
        }
    }
}
