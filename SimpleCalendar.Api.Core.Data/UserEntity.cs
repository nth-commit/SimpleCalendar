using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SimpleCalendar.Api.Core.Data
{
    public class UserEntity
    {
        [Key]
        public string Email { get; set; }

        public string ClaimsBySubJson { get; set; }

        public int ClaimsBySubVersion { get; set; }

        public string OriginatingSub { get; set; }
    }
}
