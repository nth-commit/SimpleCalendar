using Microsoft.Extensions.Configuration;
using SimpleCalendar.Utility.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class UtilityConfigurationServices
    {
        public static TServiceCollection AddConfigurationServices<TServiceCollection>(
            this TServiceCollection services,
            IConfiguration configuration)
            where TServiceCollection : IServiceCollection
        {
            services.AddSingleton<SimpleCalendar.Utility.Configuration.IConfigurationProvider>(new DefaultConfigurationProvider(configuration));

            return services;
        }

        public static TServiceCollection AddConfigurationServices<TServiceCollection>(
            this TServiceCollection services,
            string environment)
            where TServiceCollection : IServiceCollection
        {
            return services.AddConfigurationServices(ConfigurationFactory.Create(environment));
        }
    }
}
