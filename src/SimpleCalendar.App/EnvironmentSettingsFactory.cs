using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SimpleCalendar.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleCalendar.App
{
    public interface IEnvironmentSettingsFactory
    {
        object Create(HttpContext context);
    }

    public static class EnvironmentSettingsFactoryExtensions
    {
        public static string CreateJson(this IEnvironmentSettingsFactory environmentSettingsFactory, HttpContext context)
        {
            return JsonConvert.SerializeObject(environmentSettingsFactory.Create(context));
        }
    }

    public class EnvironmentSettingsFactory : IEnvironmentSettingsFactory
    {
        private readonly Auth0AuthOptions _auth0AuthOptions;

        public EnvironmentSettingsFactory(
            IOptions<Auth0AuthOptions> auth0AuthOptions)
        {
            _auth0AuthOptions = auth0AuthOptions.Value;
        }

        public object Create(HttpContext context)
        {
            return new
            {
                auth = new
                {
                    auth0 = GetPublicAuthOptions(_auth0AuthOptions)
                }
            };
        }

        private object GetPublicAuthOptions(IAuthOptions options)
        {
            return new
            {
                clientId = options.ClientId,
                domain = options.Domain
            };
        }
    }
}
