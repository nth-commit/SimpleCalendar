using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SimpleCalendar.Utility.Configuration;
using SimpleCalendar.Utility.DependencyInjection;
using SimpleCalendar.Utility.WindowsAzure.Storage;
using SimpleCalendar.Utility.WindowsAzure.Storage.Impl;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleCalendar.Utility.DependencyInjection
{
    public static class SharedWindowsAzureStorageServices
    {
        public static IValidatableServiceCollection AddWindowsAzureStorageServices(
            this IValidatableServiceCollection services)
        {
            services.AddTransient<ICloudStorageClientFactory, CloudStorageClientFactory>();

            services.AddRequirement<IConfiguration>();

            return services;
        }
    }
}
