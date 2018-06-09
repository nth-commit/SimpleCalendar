using Microsoft.Extensions.DependencyInjection;
using SimpleCalendar.Api.Core.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleCalendar.Api.UnitTests
{
    public static class GivenAnyContextExtensions
    {
        public static CoreDbContext GetCoreDbContext(this GivenAnyContext context)
            => context.GetRequiredService<CoreDbContext>();

        public static TService GetRequiredService<TService>(this GivenAnyContext context)
            => context.Services.GetRequiredService<TService>();

    }
}
