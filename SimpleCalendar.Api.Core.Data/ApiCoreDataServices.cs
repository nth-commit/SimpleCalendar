using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SimpleCalendar.Api.Core.Data;
using System.Linq;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ApiCoreDataServices
    {
        public static IServiceCollection AddApiCoreDataServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddEntityFrameworkSqlServer();

            if (!services.Any(s => s.ServiceType == typeof(CoreDbContext)))
            {
                services.AddDbContext<CoreDbContext>(options =>
                    options.UseSqlServer(configuration.GetConnectionString(typeof(CoreDbContext))));
            }

            return services;
        }
    }
}
