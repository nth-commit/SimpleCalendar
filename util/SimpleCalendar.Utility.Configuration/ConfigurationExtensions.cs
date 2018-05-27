using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.Configuration
{
    public static class ConfigurationExtensions
    {
        public static string GetConnectionString(this IConfiguration configuration, Type markerType)
            => configuration.GetConnectionString(markerType.Namespace);
    }
}
