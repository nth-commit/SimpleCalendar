using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleCalendar.Utiltiy.Validation
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class ValidateCustomBehaviourAttribute : Attribute
    {
    }
}
