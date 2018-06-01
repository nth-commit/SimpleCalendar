using SimpleCalendar.Framework.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCalendar.Api.Core.Data
{
    public class RegionRoleEntity
    {
        public string Id { get; set; }

        public string RegionId { get; set; }

        public RegionEntity Region { get; set; }

        public Role Role { get; set; }

        public string UserId { get; set; }
    }
}
