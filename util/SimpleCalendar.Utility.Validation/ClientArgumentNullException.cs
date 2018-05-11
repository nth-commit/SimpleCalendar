using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCalendar.Utiltiy.Validation
{
    public class ClientArgumentNullException : ClientArgumentException
    {
        public ClientArgumentNullException(string paramName) : base(paramName) { }
    }
}
