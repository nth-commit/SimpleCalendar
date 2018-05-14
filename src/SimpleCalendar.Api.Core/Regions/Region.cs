using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleCalendar.Api.Core.Regions
{
    public class Region
    {
        public string LocalId { get; set; }

        public string ParentId { get; set; }

        public string Id => string.IsNullOrEmpty(ParentId) ? LocalId : $"{ParentId}.{LocalId}";

        public Dictionary<string, IEnumerable<RegionRole>> RolesByUser { get; set; }
    }
}
