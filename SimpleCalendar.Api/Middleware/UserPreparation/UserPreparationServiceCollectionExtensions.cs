using Microsoft.Extensions.DependencyInjection.Extensions;
using SimpleCalendar.Api.Middleware.UserPreparation;
using SimpleCalendar.Api.Middleware.UserPreparation.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class UserPreparationServiceCollectionExtensions
    {
        public static IServiceCollection AddUserPreparation(
            this IServiceCollection services)
        {
            services.TryAddTransient<IUserInfoService, UserInfoService>();
            return services;
        }
    }
}
