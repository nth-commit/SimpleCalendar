using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SimpleCalendar.Api.Commands.Regions;
using SimpleCalendar.Api.Commands.Regions.Impl.Get;
using SimpleCalendar.Api.Commands.Regions.Impl.Create;
using SimpleCalendar.Api.Commands.Regions.Impl;
using SimpleCalendar.Api.Commands.Regions.Impl.Query;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class RegionServices
    {
        public static IServiceCollection AddRegionServices(this IServiceCollection services) =>
            services
                .AddLazyScoped<IGetRegionCommand, GetRegionCommand>()
                .AddLazyScoped<IQueryRegionCommand, QueryRegionCommand>()
                .AddLazyScoped<ICreateRegionCommand, CreateRegionCommand>()
                .AddTransient<RegionMapper>();
    }
}
