using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SimpleCalendar.Utility.Configuration;
using SimpleCalendar.Utility.DependencyInjection;

namespace SimpleCalendar.Api
{
    public class Startup
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public Startup(
            IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        public IServiceProvider ConfigureServices(IServiceCollection innerServices)
        {
            var services = new ValidatableServiceCollection(innerServices);

            services.AddApiCoreServices();
            services.AddWindowsAzureStorageServices();
            services.AddConfigurationServices(_hostingEnvironment.EnvironmentName);

            services.AddAutoMapper(conf =>
            {
                conf.AddApiCoreMappers();
            });

            services.ValidateRequirements();
            return services.BuildServiceProvider();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hello World!");
            });
        }
    }
}
