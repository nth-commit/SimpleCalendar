using AutoMapper;
using SimpleCalendar.Api.Core.Data;
using SimpleCalendar.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleCalendar.Api.Commands.Events.Impl
{
    public class EventMapper
    {
        private readonly IMapper _mapper;

        public EventMapper(
            IMapper mapper)
        {
            _mapper = mapper;
        }

        public EventOutput MapToOutput(EventEntity entity)
        {
            return MapToResult(_mapper.Map<Event>(entity));
        }

        public EventOutput MapToResult(Event ev)
        {
            return _mapper.Map<EventOutput>(ev);
        }

        public Event MapToEvent(EventInput ev)
        {
            return _mapper.Map<Event>(ev);
        }

        public EventEntity MapToEntity(Event ev)
        {
            return _mapper.Map<EventEntity>(ev);
        }
    }
}
