using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace SimpleCalendar.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var test = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, builder) => 
                    builder.AddCommonConfigurationSources(context.HostingEnvironment.EnvironmentName))
                .UseStartup<Startup>()
                .Build();
    }
}
