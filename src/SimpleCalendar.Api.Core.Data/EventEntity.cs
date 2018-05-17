using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCalendar.Api.Core.Data
{
    public class EventEntity
    {
        public string Id { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public string RegionId { get; set; }

        public RegionEntity Region { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsPublished { get; set; }

        public string DataJson { get; set; }

        public int DataJsonVersion { get; set; }
    }
}
