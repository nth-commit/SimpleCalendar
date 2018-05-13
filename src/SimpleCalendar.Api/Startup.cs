using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SimpleCalendar.Framework;
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

            services.AddMvc();

            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    var sp = services.BuildServiceProvider();
                    var auth0AuthOptions = sp.GetRequiredService<IOptions<Auth0AuthOptions>>().Value;
                    options.Authority = auth0AuthOptions.GetAuthority();
                    options.Audience = auth0AuthOptions.ClientId;
                });

            services.AddTransient<ConfigureJwtBearerOptions>();

            services.AddApiCoreServices();
            services.AddWindowsAzureStorageServices();
            services.AddConfigurationServices(_hostingEnvironment.EnvironmentName);

            services.AddAutoMapper(conf =>
            {
                conf.AddApiCoreMappers();
            });

            services.ConfigureFromProvider<Auth0AuthOptions>("Auth0");
            services.ConfigureFromProvider<HostsOptions>("Hosts");

            services.ValidateRequirements();
            return services.BuildServiceProvider();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app,
            IHostingEnvironment env,
            ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();

            if (!env.IsDevelopment())
            {
                app.UseRewriter(new RewriteOptions().AddRedirectToHttpsPermanent());
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(builder => builder
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowAnyOrigin()
                .AllowCredentials());

            app.UseAuthentication();

            app.UseMvc();
        }
    }
}
