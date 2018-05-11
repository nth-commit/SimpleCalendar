using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleCalendar.Utility.Configuration
{
    public static class IConfigurationProviderExtensions
    {
        public static string GetConnectionString(
            this IConfigurationProvider configurationProvider,
            Type markerType) =>
                configurationProvider.Configuration.GetConnectionString(markerType.Namespace);
    }
}
