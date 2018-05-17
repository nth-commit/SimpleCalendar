using AutoMapper;
using Newtonsoft.Json;
using SimpleCalendar.Api.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleCalendar.Api.Core.Events
{
    public class EventMappingProfile : Profile
    {
        public EventMappingProfile()
        {
            CreateMap<EventCreate, Event>()
                .ForMember(dest => dest.Id, opts => opts.ResolveUsing(src => Guid.NewGuid()));

            CreateMap<Event, EventEntity>()
                .ForMember(dest => dest.IsDeleted, opts => opts.ResolveUsing(src => src.Deleted.HasValue))
                .ForMember(dest => dest.IsPublished, opts => opts.ResolveUsing(src => src.Published.HasValue))
                .ForMember(dest => dest.DataJson, opts => opts.ResolveUsing(src => JsonConvert.SerializeObject(src)))
                .ForMember(dest => dest.DataJsonVersion, opts => opts.UseValue(1));

            CreateMap<EventEntity, Event>()
                .ConvertUsing((src, dest) => JsonConvert.DeserializeObject<Event>(src.DataJson));

            CreateMap<Event, EventResult>();            
        }
    }

    public static class MapperExtensions
    {
        public static EventResult MapEntityToResult(this IMapper mapper, EventEntity entity, RegionEntity region)
        {
            return mapper.MapToResult(mapper.Map<Event>(entity), region);
        }

        public static EventResult MapToResult(this IMapper mapper, Event ev, RegionEntity region)
        {
            var result = mapper.Map<EventResult>(ev);
            result.RegionId = string.Join(".", region.GetRegions().Select(r => r.Code));
            return result;
        }
    }
}
