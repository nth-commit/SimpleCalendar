using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using SimpleCalendar.Api.Core.Data;
using SimpleCalendar.Api.Test.Data;
using SimpleCalendar.Api.Test.Data.Initializers;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class TestDataServices
    {
        public static IServiceCollection AddTestData<TStartup>(this IServiceCollection services)
            where TStartup : class
        {
            var databaseRoot = new InMemoryDatabaseRoot();

            services.AddDbContext<CoreDbContext>(optionsBuilder => 
                optionsBuilder.UseInMemoryDatabase(databaseName: "Test.Data", databaseRoot: databaseRoot));

            services.AddSingleton<ITestDataInitializer<TStartup>>(sp =>
                ActivatorUtilities.CreateInstance<TestDataInitializer<TStartup>>(sp, databaseRoot));

            services.AddTransient<IDbContextTestDataInitializer, RegionInitializer>();

            services.AddTransient<IHttpClientTestDataInitializer, RegionMembershipInitializer>();

            return services;
        }
    }
}
