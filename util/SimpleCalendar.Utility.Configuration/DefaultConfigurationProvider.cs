using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace SimpleCalendar.Utility.Configuration
{
    public class DefaultConfigurationProvider : IConfigurationProvider
    {
        private readonly IConfiguration _configuration;

        public DefaultConfigurationProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IConfiguration Configuration => _configuration;
    }
}
