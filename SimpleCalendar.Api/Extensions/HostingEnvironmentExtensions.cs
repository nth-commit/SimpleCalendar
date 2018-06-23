using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Hosting
{
    public static class HostingEnvironmentExtensions
    {
        public static bool IsUnitTest(this IHostingEnvironment hostingEnvironment)
            => hostingEnvironment.EnvironmentName == "UnitTests";
    }
}
