using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleCalendar.Utility.Configuration
{
    public class ConfigureOptionsFromProvider<TOptions> : IConfigureOptions<TOptions>
        where TOptions : class
    {
        private readonly IConfigurationProvider _configurationProvider;
        private readonly string _name;

        public ConfigureOptionsFromProvider(IConfigurationProvider configurationProvider, string name)
        {
            _configurationProvider = configurationProvider;
            _name = name;
        }


        public void Configure(TOptions options)
        {
            _configurationProvider.Configuration.Bind(_name, options);
        }
    }
}
