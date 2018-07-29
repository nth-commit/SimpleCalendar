using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleCalendar.Api.UnitTests.Utililty
{
    public interface IValueContainer<T>
    {
        T Value { get; set; }
    }
}
