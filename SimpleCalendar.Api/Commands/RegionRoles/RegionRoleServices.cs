using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SimpleCalendar.Api.Commands.RegionRoles;
using SimpleCalendar.Api.Commands.RegionRoles.Impl.Query;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class RegionRoleServices
    {
        public static IServiceCollection AddRegionRoleServices(this IServiceCollection services) =>
            services
                .AddLazyScoped<IQueryRegionRoleCommand, QueryRegionRoleCommand>();
    }
}
