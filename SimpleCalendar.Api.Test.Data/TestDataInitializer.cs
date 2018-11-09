using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using SimpleCalendar.Api.Core.Data;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace SimpleCalendar.Api.Test.Data
{
    internal class TestDataInitializer<TStartup> : ITestDataInitializer<TStartup>
        where TStartup : class
    {
        private readonly ILoggerFactory _loggerFactory;
        private readonly IEnumerable<IHttpClientTestDataInitializer> _httpClientTestDataInitalizers;
        private readonly IEnumerable<IDbContextTestDataInitializer> _dbContextTestDataInitializers;
        private readonly InMemoryDatabaseRoot _databaseRoot;

        public TestDataInitializer(
            ILoggerFactory loggerFactory,
            IEnumerable<IHttpClientTestDataInitializer> httpClientTestDataInitalizers,
            IEnumerable<IDbContextTestDataInitializer> dbContextTestDataInitializers,
            InMemoryDatabaseRoot databaseRoot)
        {
            _loggerFactory = loggerFactory;
            _httpClientTestDataInitalizers = httpClientTestDataInitalizers;
            _dbContextTestDataInitializers = dbContextTestDataInitializers;
            _databaseRoot = databaseRoot;
        }

        public async Task Initialize()
        {
            var testServer = BuildTestServer();

            var coreDbContext = testServer.Host.Services.GetRequiredService<CoreDbContext>();
            await coreDbContext.Database.EnsureCreatedAsync();
            foreach (var initializer in _dbContextTestDataInitializers)
            {
                await initializer.Initialize(coreDbContext);
            }

            var client = testServer.CreateClient();
            foreach (var initializer in _httpClientTestDataInitalizers)
            {
                await initializer.Initialize(client);
            }
        }

        private TestServer BuildTestServer()
        {
            var webHostBuilder = new WebHostBuilder();
            ConfigureWebHost(webHostBuilder);

            var testServer = new TestServer(webHostBuilder);
            return testServer;
        }

        protected void ConfigureWebHost(IWebHostBuilder webHostBuilder)
        {
            webHostBuilder
                .UseStartup<TStartup>()
                .UseEnvironment("UnitTests")
                .ConfigureAppConfiguration(ConfigureAppConfiguration)
                .ConfigureServices(ConfigureServices);
        }

        protected void ConfigureAppConfiguration(IConfigurationBuilder config)
        {

        }

        protected virtual void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(new Mock<ITestDataInitializer<TStartup>>().Object);

            services.AddDbContext<CoreDbContext>(optionsBuilder =>
                optionsBuilder.UseInMemoryDatabase(databaseName: "Test.Data", databaseRoot: _databaseRoot));

            services.AddSingleton(_loggerFactory);

            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = "TestScheme";
                    options.DefaultChallengeScheme = "TestScheme";
                })
                .AddScheme<AuthenticationSchemeOptions, StubbedAuthenticationHandler>("TestScheme", options =>
                {
                });

            services.AddTransient<IUserInfoService, StubbedUserInfoService>();
        }

        public class StubbedAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
        {
            public StubbedAuthenticationHandler(
                IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
                    : base(options, logger, encoder, clock)
            {
            }

            protected override Task<AuthenticateResult> HandleAuthenticateAsync()
            {
                var identity = new ClaimsIdentity("TestAuthenticationType");
                var principal = new ClaimsPrincipal(identity);
                return Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(principal, "TestScheme")));
            }
        }

        public class StubbedUserInfoService : IUserInfoService
        {
            public Task<IEnumerable<Claim>> GetUserInfo(HttpContext httpContext)
            {
                return Task.FromResult(new Claim[]
                {
                    new Claim("email", "michaelfry2002@gmail.com"),
                    new Claim("sub", "sub")
                }.AsEnumerable());
            }
        }
    }
}
