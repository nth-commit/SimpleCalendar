using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SimpleCalendar.Api.Core.Data;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace SimpleCalendar.Api.UnitTests
{
    public class GivenAnyContext
    {
        protected HttpClient Client { get; private set; }

        protected IServiceProvider Services { get; private set; }

        public GivenAnyContext()
        {
            var webHostBuilder = new WebHostBuilder()
                .UseEnvironment("Development")
                .ConfigureAppConfiguration((context, builder) => new ConfigurationBuilder().Build())
                .UseStartup<UnitTestStartup>()
                .ConfigureServices(services =>
                {
                    services.AddEntityFrameworkInMemoryDatabase();
                    services.AddDbContext<CoreDbContext>(optionsBuilder => optionsBuilder.UseInMemoryDatabase(databaseName: "Test"));
                });

            var testServer = new TestServer(webHostBuilder);
            Services = testServer.Host.Services;
            Client = testServer.CreateClient();
        }
    }
}
