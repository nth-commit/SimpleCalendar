using SimpleCalendar.Api.Core.Data;
using SimpleCalendar.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoMapper
{
    public static class RegionRoleMappers
    {
        public static void AddRegionRoleMappers(
            this IMapperConfigurationExpression conf)
        {
            conf.CreateMap<RegionRoleEntity, RegionRole>();
        }
    }
}
