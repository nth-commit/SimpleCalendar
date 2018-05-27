using Microsoft.Extensions.Options;
using SimpleCalendar.Utility.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class UtilityConfigurationServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureFromAppConfigurationSection<TOptions>(
            this IServiceCollection services,
            string key)
            where TOptions : class
        {
            services.AddSingleton<IConfigureOptions<TOptions>>(sp =>
                ActivatorUtilities.CreateInstance<ConfigureFromAppConfigurationSection<TOptions>>(sp, key));

            return services;
        }
    }
}
