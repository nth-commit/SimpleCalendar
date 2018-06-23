using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using SimpleCalendar.Api.Core.Data;
using SimpleCalendar.Api.UnitTests.Utililty;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCalendar.Api.UnitTests
{
    public class GivenAnyContext
    {
        private readonly MockCollection _mocks = new MockCollection();

        public HttpClient Client { get; private set; }

        public IServiceProvider Services { get; private set; }

        public Mock<IUserIdContainer> UserId => _mocks.UserId;

        public GivenAnyContext() => InitializeAsync().GetAwaiter().GetResult();

        private async Task InitializeAsync()
        {
            var webHostBuilder = new WebHostBuilder()
                .UseEnvironment("UnitTests")
                .ConfigureAppConfiguration((context, builder) => new ConfigurationBuilder().Build())
                .UseStartup<UnitTestStartup>()
                .ConfigureServices(services =>
                {
                    services.AddSingleton(_mocks);
                });

            var testServer = new TestServer(webHostBuilder);
            Services = testServer.Host.Services;
            Client = testServer.CreateClient();

            await this.GetCoreDbContext().Database.EnsureCreatedAsync();
        }
    }
}
