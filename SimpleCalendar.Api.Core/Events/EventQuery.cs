using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleCalendar.Api.Core.Events
{
    public class EventQuery
    {
        public string RegionId { get; set; }

        public DateTime From { get; set; }

        public DateTime To { get; set; }
    }
}
