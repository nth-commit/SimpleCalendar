using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SimpleCalendar.Api.UnitTests
{
    public class UnitTestStartup : Startup
    {
        public UnitTestStartup(IConfiguration configuration) : base(configuration)
        {
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);

            services.AddEntityFrameworkInMemoryDatabase();
        }
    }
}
