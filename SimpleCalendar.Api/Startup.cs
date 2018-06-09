using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SimpleCalendar.Api.Services;
using SimpleCalendar.Api.Core.Data;
using SimpleCalendar.Framework;
using SimpleCalendar.Framework.Identity;
using SimpleCalendar.Utility.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication;

namespace SimpleCalendar.Api
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public virtual void ConfigureServices(IServiceCollection innerServices)
        {
            var services = new ValidatableServiceCollection(innerServices);

            services.AddMvc();

            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IUserAccessor, HttpUserAccessor>();

            ConfigureAuthenticationServices(services);

            services.AddApiCoreServices();
            services.AddApiCoreDataServices(_configuration);
            services.AddAuthorizationUtilityServices();

            services.AddAutoMapper(conf =>
            {
                conf.AddApiCoreMappers();
            });

            services.ConfigureAuth0();
            services.ConfigureHosts();

            services.ValidateRequirements();
        }

        public virtual void ConfigureAuthenticationServices(IServiceCollection services)
        {
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
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public virtual void Configure(
            IApplicationBuilder app,
            IHostingEnvironment env,
            ILoggerFactory loggerFactory,
            CoreDbContext coreDbContext)
        {
            loggerFactory.AddConsole();

            if (env.IsDevelopment())
            {
                coreDbContext.Database.EnsureCreated();
            }

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
