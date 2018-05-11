using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using SimpleCalendar.Api.Core.Events;
using SimpleCalendar.Utility.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCalendar.Tools.Runner
{
    public static class ApiCoreRunner
    {
        public static async Task RunAsync()
        {
            var services = new ValidatableServiceCollection(new ServiceCollection());

            services.AddApiCoreServices();
            services.AddWindowsAzureStorageServices();
            services.AddConfigurationServices("Development");

            services.AddAutoMapper(conf =>
            {
                conf.AddApiCoreMappers();
            });

            services.ValidateRequirements();
            var serviceProvider = services.BuildServiceProvider();

            await CreateEventAsync(serviceProvider);
        }

        public static async Task CreateEventAsync(IServiceProvider serviceProvider)
        {
            var eventService = serviceProvider.GetRequiredService<EventService>();

            var result = await eventService.CreateEventAsync(new EventCreate()
            {
                Name = "My Event",
                RegionId = "nz.wellington"
            });
        }
    }
}
