using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleCalendar.Api.Core.Events
{
    public class EventCreateResult
    {
        public enum EventCreateResultStatus
        {
            Unauthorized,
            Created,
            Published
        }

        public static EventCreateResult Unauthorized => new EventCreateResult()
        {
            Status = EventCreateResultStatus.Unauthorized
        };

        public static EventCreateResult Success(EventResult ev, bool isPublished) => new EventCreateResult()
        {
            Event = ev,
            Status = isPublished ? EventCreateResultStatus.Published : EventCreateResultStatus.Created
        };

        public EventCreateResultStatus Status { get; private set; }

        public EventResult Event { get; private set; }
    }
}
