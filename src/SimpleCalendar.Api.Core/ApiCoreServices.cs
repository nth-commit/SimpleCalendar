using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using SimpleCalendar.Api.Core.Data;
using SimpleCalendar.Api.Core.Events;
using SimpleCalendar.Api.Core.Organisation;
using SimpleCalendar.Api.Core.Regions;
using SimpleCalendar.Framework.Identity;
using SimpleCalendar.Utility.Authorization;
using SimpleCalendar.Utility.DependencyInjection;
using SimpleCalendar.Utility.WindowsAzure.Storage;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ApiCoreServices
    {
        public static IValidatableServiceCollection AddApiCoreServices(
            this IValidatableServiceCollection services)
        {
            services.AddTransient<EventService>();
            services.AddTransient<IEventRepository, TableStorageEventRepository>();

            services.AddTransient<OrganisationService>();

            services.AddTransient<RegionService>();
            services.AddTransient<RegionRoleService>();

            services.AddRequirement<IMapper>();
            services.AddRequirement<ICloudStorageClientFactory>();
            services.AddRequirement<CoreDbContext>();
            services.AddRequirement<IClaimsPrincipalAccessor>();
            services.AddRequirement<IAuthorizationService>();
            services.AddRequirement<IClaimsPrincipalAuthorizationService>();

            return services;
        }

    }
}
