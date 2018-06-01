using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleCalendar.Api.Core.Organisation
{
    public class Organisation
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<string> Regions { get; set; }

        public IEnumerable<string> Members { get; set; }
    }
}
