using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleCalendar.Api.Services
{
    public interface IDateTimeAccessor
    {
        DateTime UtcNow { get; }
    }

    public class DateTimeAccessor : IDateTimeAccessor
    {
        public DateTime UtcNow => DateTime.UtcNow;
    }
}
