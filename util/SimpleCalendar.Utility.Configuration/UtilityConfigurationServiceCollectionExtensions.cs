using Microsoft.Extensions.Options;
using SimpleCalendar.Utility.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class UtilityConfigurationServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureFromProvider<TOptions>(
            this IServiceCollection services,
            string name)
            where TOptions : class
        {
            services.AddSingleton<IConfigureOptions<TOptions>>(sp =>
                ActivatorUtilities.CreateInstance<ConfigureOptionsFromProvider<TOptions>>(sp, name));

            return services;
        }
    }
}
