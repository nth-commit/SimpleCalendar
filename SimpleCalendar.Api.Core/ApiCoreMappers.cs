using SimpleCalendar.Api.Core.Events;
using SimpleCalendar.Api.Core.Regions;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutoMapper
{
    public static class ApiCoreMappers
    {
        public static void AddApiCoreMappers(
            this IMapperConfigurationExpression conf)
        {
            conf.AddProfile<EventMappingProfile>();
            conf.AddProfile<RegionMappingProfile>();
        }
    }
}
