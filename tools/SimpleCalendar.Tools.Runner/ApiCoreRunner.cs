using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using SimpleCalendar.Api.Core.Events;
using SimpleCalendar.Api.Core.Regions;
using SimpleCalendar.Framework.Identity;
using SimpleCalendar.Utility.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCalendar.Tools.Runner
{
    public static class ApiCoreRunner
    {
        public static async Task RunAsync()
        {
            var services = new ValidatableServiceCollection(new ServiceCollection());

            services.AddTransient<IUserAccessor, StubbedUserAccessor>();
            services.AddAuthorization();
            services.AddAuthorizationUtilityServices();

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

            await CreateEventAsync(serviceProvider);
            //await GetRegionAsync(serviceProvider);
        }

        public static async Task CreateEventAsync(IServiceProvider serviceProvider)
        {
            var eventService = serviceProvider.GetRequiredService<EventService>();

            var result = await eventService.CreateEventAsync(new EventCreate()
            {
                RegionId = "new_zealand.wellington",
                Name = "My Event",
                Description = "Event desc",
                StartTime = DateTime.Now,
                EndTime = DateTime.Now
            });
        }

        public static async Task GetRegionAsync(IServiceProvider serviceProvider)
        {
            var regionService = serviceProvider.GetRequiredService<RegionService>();

            var result = await regionService.GetRegionAsync("new_zealand");

            //var result = await regionService.GetRegionAsync("new_zealand.wellington.wellington_city");
            var result2 = await regionService.ListRegionsAsync("new_zealand.wellington");
        }

        public class StubbedUserAccessor : IUserAccessor
        {
            public ClaimsPrincipal User
            {
                get
                {
                    var identity = new ClaimsIdentity("test", "sub", "role");
                    identity.AddClaim(new Claim("sub", "ROOT_ADMIN"));
                    return new ClaimsPrincipal(identity);
                }
            }
        }
    }
}
