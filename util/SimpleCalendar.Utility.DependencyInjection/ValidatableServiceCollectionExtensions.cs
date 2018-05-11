using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleCalendar.Utility.DependencyInjection
{
    public static class ValidatableServiceCollectionExtensions
    {
        public static void AddRequirement<TService>(this IValidatableServiceCollection services)
        {
            services.AddRequirement(typeof(TService));
        }
    }
}
