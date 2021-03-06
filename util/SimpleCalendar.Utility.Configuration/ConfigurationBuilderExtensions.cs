﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Microsoft.Extensions.Configuration
{
    public static class ConfigurationBuilderExtensions
    {
        public static IConfigurationBuilder AddCommonConfigurationSources(
            this IConfigurationBuilder builder,
            string environmentName)
        {
            builder
                .AddJsonFile("appsettings.json", optional: false)
                .AddJsonFile($"appsettings.{environmentName}.json", optional: true)
                .AddJsonFile("appsettings.Shared.json", optional: false)
                .AddJsonFile($"appsettings.Shared.{environmentName}.json", optional: true);

            if (environmentName == "Development")
            {
                builder.AddUserSecrets("SimpleCalendar");
            }

            builder.AddEnvironmentVariables();

            return builder;
        }
    }
}
