using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleCalendar.Api.UnitTests
{
    public static class GivenAnyContextExtensions
    {
        public static TService GetRequiredService<TService>(this GivenAnyContext context)
            => context.Services.GetRequiredService<TService>();
    }
}
