using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleCalendar.Utility.Configuration
{
    public class ConfigureFromAppConfigurationSection<TOptions> : IConfigureOptions<TOptions>
        where TOptions : class
    {
        private readonly IConfiguration _configuration;
        private readonly string _key;

        public ConfigureFromAppConfigurationSection(IConfiguration configuration, string key)
        {
            _configuration = configuration;
            _key = key;
        }


        public void Configure(TOptions options)
        {
            _configuration.Bind(_key, options);
        }
    }
}
