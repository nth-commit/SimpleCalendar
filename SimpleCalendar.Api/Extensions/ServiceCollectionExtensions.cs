using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddLazyScoped<TService, TImplementation>(this IServiceCollection services)
            where TService : class
            where TImplementation : class, TService
        {
            services.AddScoped<TService, TImplementation>();
            services.AddScoped(sp => new Lazy<TService>(() => sp.GetRequiredService<TService>()));
            return services;
        }
    }
}
