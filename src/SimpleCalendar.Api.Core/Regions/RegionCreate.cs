using SimpleCalendar.Utiltiy.Validation;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleCalendar.Api.Core.Regions
{
    public class RegionCreate
    {
        [StringNotNullOrEmpty]
        public string Id { get; set; }

        [StringNotNullOrEmpty]
        public string Name { get; set; }

        public Dictionary<string, IEnumerable<RegionRole>> RolesByUser { get; set; }
    }
}
