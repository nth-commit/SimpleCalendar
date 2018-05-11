using System;
using System.Threading.Tasks;

namespace SimpleCalendar.Tools.Runner
{
    class Program
    {
        static void Main(string[] args)
        {
            RunAsync().GetAwaiter().GetResult();
        }

        static async Task RunAsync()
        {
            await ApiCoreRunner.RunAsync();
        }
    }
}
