using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleCalendar.Api.Models
{
    public class RegionCreate
    {
        public string Name { get; set; }

        public string ParentId { get; set; }
    }
}
