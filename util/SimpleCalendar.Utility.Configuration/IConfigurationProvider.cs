using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleCalendar.Utility.Configuration
{
    public interface IConfigurationProvider
    {
        IConfiguration Configuration { get; }
    }
}
