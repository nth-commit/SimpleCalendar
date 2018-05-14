using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCalendar.Api.Core.Data
{
    public class Region
    {
        public string Id { get; set; }

        public string ParentId { get; set; }

        public Region Parent { get; set; }

        public string DataJson { get; set; }

        public int DataJsonVersion { get; set; }
    }
}
