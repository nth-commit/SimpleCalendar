using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using SimpleCalendar.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleCalendar.Api
{
    public class ConfigureJwtBearerOptions : IConfigureOptions<JwtBearerOptions>
    {
        private readonly Auth0AuthOptions _auth0AuthOptions;

        public ConfigureJwtBearerOptions(
            IOptions<Auth0AuthOptions> auth0AuthOptions)
        {
            _auth0AuthOptions = auth0AuthOptions.Value;
        }

        public void Configure(JwtBearerOptions options)
        {
            options.Audience = _auth0AuthOptions.ClientId;
            options.Authority = _auth0AuthOptions.GetAuthority();
        }
    }
}
