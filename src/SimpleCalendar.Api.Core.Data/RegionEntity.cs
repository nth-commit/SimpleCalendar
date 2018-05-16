using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCalendar.Api.Core.Data
{
    public class RegionEntity
    {
        public string Id { get; set; }

        public string Code { get; set; }

        public string ParentId { get; set; }

        public RegionEntity Parent { get; set; }

        public string DataJson { get; set; }

        public int DataJsonVersion { get; set; }

        [InverseProperty(nameof(Parent))]
        public List<RegionEntity> Children { get; set; }
    }
}
