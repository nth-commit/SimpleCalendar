﻿using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SimpleCalendar.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleCalendar.App2
{
    public interface IEnvironmentSettingsFactory
    {
        object Create();
    }

    public static class EnvironmentSettingsFactoryExtensions
    {
        public static string CreateJson(this IEnvironmentSettingsFactory environmentSettingsFactory)
        {
            return JsonConvert.SerializeObject(environmentSettingsFactory.Create(), Formatting.None);
        }
    }

    public class EnvironmentSettingsFactory : IEnvironmentSettingsFactory
    {
        private readonly Auth0AuthOptions _auth0AuthOptions;
        private readonly HostsOptions _hostsOptions;

        public EnvironmentSettingsFactory(
            IOptions<Auth0AuthOptions> auth0AuthOptions,
            IOptions<HostsOptions> hostsOptions)
        {
            _auth0AuthOptions = auth0AuthOptions.Value;
            _hostsOptions = hostsOptions.Value;
        }

        public object Create()
        {
            return new
            {
                Auth = new
                {
                    Auth0 = GetPublicAuthOptions(_auth0AuthOptions)
                },
                Hosts = _hostsOptions
            };
        }

        private object GetPublicAuthOptions(IAuthOptions options)
        {
            return new
            {
                 options.ClientId,
                 options.Domain
            };
        }
    }
}