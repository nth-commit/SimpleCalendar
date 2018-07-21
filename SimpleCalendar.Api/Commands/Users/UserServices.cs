using SimpleCalendar.Api.Commands.Users;
using SimpleCalendar.Api.Commands.Users.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class UserServices
    {
        public static IServiceCollection AddUserServices(this IServiceCollection services) => services
            .AddLazyScoped<IGetUserCommand, GetUserCommand>();
    }
}
