using System.IdentityModel.Tokens.Jwt;
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
using SimpleCalendar.Api.Filters;
using SimpleCalendar.Api.Services;
using SimpleCalendar.Api.Core.Data;
using SimpleCalendar.Framework;
using SimpleCalendar.Framework.Identity;
using SimpleCalendar.Utility.DependencyInjection;
using SimpleCalendar.Api.Test.Data;

namespace SimpleCalendar.Api
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly IHostingEnvironment _hostingEnvironment;

        public Startup(
            IConfiguration configuration,
            IHostingEnvironment hostingEnvironment)
        {
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
        }

        public virtual void ConfigureServices(IServiceCollection innerServices)
        {
            var services = new ValidatableServiceCollection(innerServices);

            services.AddMvc(options =>
            {
                options.Filters.Add<ModelStateValidationFilter>();
            });
            services.AddMemoryCache();

            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IUserAccessor, HttpUserAccessor>();
            services.AddScoped<IRegionRolesAccessor, RegionRolesAccessor>();
            services.AddTransient<IRegionCache, RegionCache>();
            services.AddTransient<IDateTimeAccessor, DateTimeAccessor>();

            ConfigureAuthenticationServices(services);

            if (_hostingEnvironment.IsDevelopment())
            {
                services.AddTestData<Startup>();
            }

            services.AddApiCoreDataServices(_configuration);
            services.AddAuthorizationUtilityServices();
            services.AddApiCoreAuthorizationServices();

            services.AddAutoMapper(conf =>
            {
                conf.AddRegionMappers();
                conf.AddRegionRoleMappers();
                conf.AddEventMappers();
            });

            services.ConfigureAuth0();
            services.ConfigureHosts();

            services.AddUserPreparation();

            services.AddRegionServices();
            services.AddRegionMembershipServices();
            services.AddRegionRoleServices();
            services.AddUserServices();
            services.AddEventsServices();

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
                    options.Audience = "wellingtonveganactions";

                    options.Events = new JwtBearerEvents()
                    {
                        OnTokenValidated = (context) =>
                        {
                            context.HttpContext.SetSecurityToken(((JwtSecurityToken)context.SecurityToken)?.RawData);
                            return Task.CompletedTask;
                        }
                    };
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public virtual void Configure(
            IApplicationBuilder app,
            IHostingEnvironment env,
            ILoggerFactory loggerFactory,
            CoreDbContext coreDbContext,
            ITestDataInitializer<Startup> testDataInitializer)
        {
            loggerFactory.AddConsole();

            if (env.IsDevelopment())
            {
                testDataInitializer.Initialize().GetAwaiter().GetResult();
            }

            if (!env.IsDevelopment() && !env.IsUnitTest())
            {
                app.UseRewriter(new RewriteOptions().AddRedirectToHttpsPermanent());
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseCustomExceptionHandler();
            }

            app.UseCors(builder => builder
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowAnyOrigin()
                .AllowCredentials());

            app.UseAuthentication();

            app.UseUserPreparation();

            app.UseMvc();
        }
    }
}
