using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleCalendar.Api.Core.Events
{
    public class EventGetResult
    {
        public enum EventGetResultStatus
        {
            Success,
            Unauthorized,
            NotFound
        }

        public static EventGetResult Unauthorized => new EventGetResult()
        {
            Status = EventGetResultStatus.Unauthorized
        };

        public static EventGetResult NotFound => new EventGetResult()
        {
            Status = EventGetResultStatus.NotFound
        };

        public static EventGetResult Success(EventResult ev) => new EventGetResult()
        {
            Event = ev,
            Status = EventGetResultStatus.Success
        };

        public EventGetResultStatus Status { get; private set; }

        public EventResult Event { get; private set; }
    }
}
