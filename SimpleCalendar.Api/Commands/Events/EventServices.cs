using SimpleCalendar.Api.Commands.Events;
using SimpleCalendar.Api.Commands.Events.Impl;
using SimpleCalendar.Api.Commands.Events.Impl.Create;
using SimpleCalendar.Api.Commands.Events.Impl.Get;
using SimpleCalendar.Api.Commands.Events.Impl.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class EventsServices
    {
        public static IServiceCollection AddEventsServices(this IServiceCollection services) =>
            services
                .AddLazyScoped<IQueryEventsCommand, QueryEventsCommand>()
                .AddLazyScoped<IGetEventsCommand, GetEventsCommand>()
                .AddLazyScoped<ICreateEventsCommand, CreateEventsCommand>()
                .AddTransient<EventMapper>();
    }
}
