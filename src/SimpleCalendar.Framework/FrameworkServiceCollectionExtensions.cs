using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCalendar.Framework
{
    public static class FrameworkServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureAuth0(
            this IServiceCollection services)
        {
            services.ConfigureFromAppConfigurationSection<HostsOptions>("Hosts");
            return services;
        }

        public static IServiceCollection ConfigureHosts(
            this IServiceCollection services)
        {
            services.ConfigureFromAppConfigurationSection<HostsOptions>("Hosts");
            return services;
        }
    }
}
