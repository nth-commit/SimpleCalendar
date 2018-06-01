using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCalendar.Api.Core.Initializer
{
    public interface IDataInitializer
    {
        Task RunAsync();
    }
}
