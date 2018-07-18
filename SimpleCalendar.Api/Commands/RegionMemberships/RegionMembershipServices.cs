using SimpleCalendar.Api.Commands;
using SimpleCalendar.Api.Commands.RegionMemberships;
using SimpleCalendar.Api.Commands.RegionMemberships.Impl.Create;
using SimpleCalendar.Api.Commands.RegionMemberships.Impl.Delete;
using SimpleCalendar.Api.Commands.RegionMemberships.Impl.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class RegionMembershipServices
    {
        public static IServiceCollection AddRegionMembershipServices(this IServiceCollection services) =>
            services
                .AddLazyScoped<IQueryRegionMembershipCommand, QueryRegionMembershipCommand>()
                .AddLazyScoped<ICreateRegionMembershipCommand, CreateRegionMembershipCommand>()
                .AddLazyScoped<IDeleteRegionMembershipCommand, DeleteRegionMembershipCommand>();
    }
}
