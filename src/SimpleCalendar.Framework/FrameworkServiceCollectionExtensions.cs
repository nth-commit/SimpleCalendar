using SimpleCalendar.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class FrameworkServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureAuth0(
            this IServiceCollection services)
        {
            services.ConfigureFromAppConfigurationSection<Auth0AuthOptions>("Auth");
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
