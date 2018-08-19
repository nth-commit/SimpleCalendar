using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleCalendar.Api.Commands.Events
{
    public class Event
    {
        public string Id { get; set; }

        public string RegionId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public DateTime Created { get; set; }

        public string CreatedByEmail { get; set; }

        public DateTime? Published { get; set; }

        public string PublishedByEmail { get; set; }

        public DateTime? Deleted { get; set; }

        public string DeletedByEmail { get; set; }
    }
}
