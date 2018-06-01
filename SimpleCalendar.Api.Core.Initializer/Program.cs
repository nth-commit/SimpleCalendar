using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using SimpleCalendar.Api.Core.Data;
using SimpleCalendar.Api.Core.Initializer.Regions;
using SimpleCalendar.Utility.Configuration;
using SimpleCalendar.Utility.DependencyInjection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimpleCalendar.Api.Core.Initializer
{
    class Program
    {
        static void Main(string[] args)
        {
            var configuration = ConfigurationFactory.Create("Development");

            var services = new ValidatableServiceCollection();

            services.AddSingleton(configuration);
            services.AddApiCoreServices();
            services.AddApiCoreDataServices(configuration);

            services.AddAutoMapper(conf =>
            {
                conf.AddApiCoreMappers();
            });

            services.AddTransient<IDataInitializer, RegionDataInitializer>();

            var serviceProvider = services.BuildServiceProvider();
            RunDataInitializers(serviceProvider).GetAwaiter().GetResult();
        }

        static async Task RunDataInitializers(IServiceProvider serviceProvider)
        {
            var coreDbContext = serviceProvider.GetRequiredService<CoreDbContext>();
            await coreDbContext.Database.EnsureCreatedAsync();

            var dataInitializers = serviceProvider.GetRequiredService<IEnumerable<IDataInitializer>>();
            foreach (var dataInitializer in dataInitializers)
            {
                await dataInitializer.RunAsync();
            }
        }
    }
}
