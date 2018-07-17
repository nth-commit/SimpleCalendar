using SimpleCalendar.Api.Commands;
using SimpleCalendar.Api.Commands.RegionMemberships;
using SimpleCalendar.Api.Commands.RegionMemberships.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class RegionMembershipCommandServices
    {
        public static IServiceCollection AddRegionMembershipCommands(this IServiceCollection services) =>
            services
                .AddLazyScoped<IQueryRegionMembershipCommand, QueryRegionMembershipCommand>()
                .AddLazyScoped<ICreateRegionMembershipCommand, CreateRegionMembershipCommand>()
                .AddLazyScoped<IDeleteRegionMembershipCommand, DeleteRegionMembershipCommand>();
    }
}
