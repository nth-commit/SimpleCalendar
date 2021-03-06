﻿using Newtonsoft.Json;
using SimpleCalendar.Api.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace System.Net.Http
{
    public static class EventHttpResponseMessageExtensions
    {
        public static async Task<IEnumerable<EventOutput>> DeserializeEventsAsync(this HttpResponseMessage response)
            => JsonConvert.DeserializeObject<IEnumerable<EventOutput>>(await response.Content.ReadAsStringAsync());
    }
}
