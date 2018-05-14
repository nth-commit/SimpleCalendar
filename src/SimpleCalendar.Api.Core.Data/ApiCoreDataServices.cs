using Microsoft.EntityFrameworkCore;
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
            this IServiceCollection services)
        {
            services.AddDbContext<CoreDbContext>(options =>
            {
                var configurationProvider = services.BuildServiceProvider().GetRequiredService<IConfigurationProvider>();
                options.UseSqlServer(configurationProvider.GetConnectionString(typeof(CoreDbContext)));
            });
            return services;
        }
    }
}
