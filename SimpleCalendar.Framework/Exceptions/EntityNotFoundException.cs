using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace SimpleCalendar.Framework.Exceptions
{
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException()
        {
        }

        public EntityNotFoundException(string message) : base(message)
        {
        }
    }
}
