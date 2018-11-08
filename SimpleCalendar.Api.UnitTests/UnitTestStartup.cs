using System;
using System.Linq;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using MoreLinq;
using SimpleCalendar.Api.Core.Data;
using SimpleCalendar.Api.UnitTests.Utililty;

namespace SimpleCalendar.Api.UnitTests
{
    public class UnitTestStartup : Startup
    {
        private readonly MockCollection _mockCollection;

        public UnitTestStartup(
            IConfiguration configuration,
            IHostingEnvironment hostingEnvironment,
            MockCollection mockCollection)
            : base(configuration, hostingEnvironment)
        {
            _mockCollection = mockCollection;
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            // Need to add EntityFramework services before Startup services, as first configuration counts for EF
            services.AddEntityFrameworkInMemoryDatabase();
            services.AddDbContext<CoreDbContext>(optionsBuilder => optionsBuilder.UseInMemoryDatabase(databaseName: "UnitTests"));

            base.ConfigureServices(services);

            typeof(MockCollection).GetProperties().ForEach(p =>
            {
                if (p.PropertyType.GetGenericTypeDefinition() != typeof(Mock<>))
                {
                    throw new Exception($"Properties on {typeof(MockCollection).FullName} must implement {typeof(Mock<>).FullName}");
                }

                services.AddSingleton(p.PropertyType.GetGenericArguments().First(), (((Mock)p.GetValue(_mockCollection)).Object));
            });
        }

        public override void ConfigureAuthenticationServices(IServiceCollection services)
        {
            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = "TestScheme";
                    options.DefaultChallengeScheme = "TestScheme";
                })
                .AddScheme<AuthenticationSchemeOptions, UserEmailAuthenticationHandler>("TestScheme", options =>
                {
                });
        }
    }
}
