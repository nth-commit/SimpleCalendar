using Newtonsoft.Json;
using SimpleCalendar.Api.Core.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace System.Net.Http
{
    public static class EventHttpResponseMessageExtensions
    {
        public static async Task<IEnumerable<EventResult>> DeserializeEventsAsync(this HttpResponseMessage response)
            => JsonConvert.DeserializeObject<IEnumerable<EventResult>>(await response.Content.ReadAsStringAsync());
    }
}
