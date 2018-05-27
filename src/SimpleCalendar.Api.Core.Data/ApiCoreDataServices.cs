using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SimpleCalendar.Api.Core.Data;
using SimpleCalendar.Utility.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ApiCoreDataServices
    {
        public static IServiceCollection AddApiCoreDataServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<CoreDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString(typeof(CoreDbContext)));
            });
            return services;
        }
    }
}
