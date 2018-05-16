using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using SimpleCalendar.Api.Core.Events;
using SimpleCalendar.Api.Core.Regions;
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
            services.AddApiCoreDataServices();
            services.AddWindowsAzureStorageServices();
            services.AddConfigurationServices("Development");

            services.AddAutoMapper(conf =>
            {
                conf.AddApiCoreMappers();
            });

            services.ValidateRequirements();
            var serviceProvider = services.BuildServiceProvider();

            //await CreateEventAsync(serviceProvider);
            await GetRegionAsync(serviceProvider);
        }

        public static async Task CreateEventAsync(IServiceProvider serviceProvider)
        {
            var eventService = serviceProvider.GetRequiredService<EventService>();

            var result = await eventService.CreateEventAsync(new EventCreate()
            {
                Name = "My Event",
                RegionId = "new_zealand.wellington"
            });
        }

        public static async Task GetRegionAsync(IServiceProvider serviceProvider)
        {
            var regionService = serviceProvider.GetRequiredService<RegionService>();

            var result = await regionService.GetRegionAsync("new_zealand.wellington");
            var result2 = await regionService.ListRegionsAsync("new_zealand.wellington");
        }
    }
}
