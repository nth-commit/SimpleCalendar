using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCalendar.Api.Core.Data
{
    public class RegionMembershipEntity
    {
        public string Id { get; set; }

        public string RegionId { get; set; }

        public RegionEntity Region { get; set; }

        public string RegionRoleId { get; set; }

        public RegionRoleEntity RegionRole { get; set; }

        public string UserEmail { get; set; }
    }
}
