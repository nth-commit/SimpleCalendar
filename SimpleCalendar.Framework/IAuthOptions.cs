using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCalendar.Framework
{
    public interface IAuthOptions
    {
        string Domain { get; set; }

        string ClientId { get; set; }

        string ClientSecret { get; set; }
    }

    public static class AuthOptionsExtensions
    {
        public static string GetAuthority(this IAuthOptions options)
            => $"https://{options.Domain}";
    }
}
