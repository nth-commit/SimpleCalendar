using AutoMapper;
using SimpleCalendar.Api.Core.Events;
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

            services.AddRequirement<IMapper>();
            services.AddRequirement<ICloudStorageClientFactory>();

            return services;
        }

    }
}
