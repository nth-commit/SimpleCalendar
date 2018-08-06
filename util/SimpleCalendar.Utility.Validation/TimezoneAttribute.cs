using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace SimpleCalendar.Utility.Validation
{
    public class TimezoneAttribute : ValidationAttribute
    {
        private static HashSet<string> TimezoneIds =
            new HashSet<string>(TimeZoneInfo.GetSystemTimeZones().Select(t => t.Id));

        public override bool IsValid(object value)
        {
            if (value == null)
            {
                // Defer to RequiredAttribute
                return true;
            }

            if (!(value is string valueStr))
            {
                throw new Exception();
            }

            return TimezoneIds.Contains(valueStr);
        }
    }
}
