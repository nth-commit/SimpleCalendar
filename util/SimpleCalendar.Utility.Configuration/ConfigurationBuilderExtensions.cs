using System;
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
            var solutionDirectory = GetSolutionDirectory(new DirectoryInfo(Directory.GetCurrentDirectory()));

            builder
                .AddJsonFile("appsettings.json", optional: false)
                .AddJsonFile($"appsettings.{environmentName}.json", optional: true)
                .AddJsonFile(Path.Combine(solutionDirectory, "appsettings.Shared.json"), optional: false)
                .AddJsonFile(Path.Combine(solutionDirectory, $"appsettings.Shared.{environmentName}.json"), optional: true);

            if (environmentName == "Development")
            {
                builder.AddUserSecrets("SimpleCalendar");
            }

            builder.AddEnvironmentVariables();

            return builder;
        }

        private static string GetSolutionDirectory(DirectoryInfo directory)
        {
            return Directory.EnumerateFiles(directory.FullName, "*.sln").Any() ? directory.FullName : GetSolutionDirectory(directory.Parent);
        }
    }
}
