using SimpleCalendar.Utiltiy.Validation;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleCalendar.Api.Core.Regions
{
    public class RegionCreate
    {
        public string Name { get; set; }

        public string ParentId { get; set; }
    }
}
