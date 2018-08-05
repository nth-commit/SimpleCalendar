using Newtonsoft.Json;
using SimpleCalendar.Api.Core.Data;
using SimpleCalendar.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoMapper
{
    public static class EventMappers
    {
        public static void AddEventMappers(
            this IMapperConfigurationExpression conf)
        {
            conf.CreateMap<EventInput, Event>()
                .ForMember(dest => dest.Id, opts => opts.ResolveUsing(src => Guid.NewGuid()));

            conf.CreateMap<Event, EventEntity>()
                .ForMember(dest => dest.IsDeleted, opts => opts.ResolveUsing(src => src.Deleted.HasValue))
                .ForMember(dest => dest.IsPublished, opts => opts.ResolveUsing(src => src.Published.HasValue))
                .ForMember(dest => dest.DataJson, opts => opts.ResolveUsing(src => JsonConvert.SerializeObject(src)))
                .ForMember(dest => dest.DataJsonVersion, opts => opts.UseValue(1));

            conf.CreateMap<EventEntity, Event>()
                .ConvertUsing((src, dest) => JsonConvert.DeserializeObject<Event>(src.DataJson));

            conf.CreateMap<Event, EventOutput>();
        }
    }
}
