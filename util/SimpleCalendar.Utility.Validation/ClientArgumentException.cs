using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCalendar.Utiltiy.Validation
{
    public class ClientArgumentException : ClientValidationException
    {
        public string ParamName { get; private set; }

        public string Readon { get; private set; }

        public ClientArgumentException(string paramName, string reason = null) : base(GetMessage(paramName, reason))
        {
            ParamName = paramName;
            Readon = reason;
        }

        private static string GetMessage(string paramName, string reason)
        {
            var message = $"Parameter {paramName} is invalid";
            if (!string.IsNullOrEmpty(reason))
            {
                message += $". {reason}.";
            }
            return message;
        }
    }
}
