﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using SimpleCalendar.Api.Core.Data;
using SimpleCalendar.Api.Core.Events;
using SimpleCalendar.Framework.Identity;
using SimpleCalendar.Utility.Authorization;
using SimpleCalendar.Utility.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ApiCoreServices
    {
        public static IValidatableServiceCollection AddApiCoreServices(
            this IValidatableServiceCollection services)
        {
            services.AddTransient<EventService>();
            services.AddTransient<IEventQueryService, EventQueryService>();

            services.AddRequirement<IMapper>();
            services.AddRequirement<CoreDbContext>();
            services.AddRequirement<IUserAccessor>();
            services.AddRequirement<IAuthorizationService>();
            services.AddRequirement<IUserAuthorizationService>();

            return services;
        }

    }
}
