using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleCalendar.Utility.Configuration
{
    public static class ConfigurationFactory
    {
        public static IConfiguration Create(string environment = "Development")
        {
            return new ConfigurationBuilder().AddCommonConfigurationSources(environment).Build();
        }
    }
}
