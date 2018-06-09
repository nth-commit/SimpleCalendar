using Newtonsoft.Json;
using SimpleCalendar.Api.Core.Events;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCalendar.Api.UnitTests.Events
{
    public static class HttpClientExtensions
    {
        public static async Task<HttpResponseMessage> CreateEventAsync(
            this HttpClient client,
            EventCreate create)
        {
            var content = new StringContent(JsonConvert.SerializeObject(create), Encoding.UTF8, "application/json");
            return await client.PostAsync("/events", content);
        }

        public static async Task<HttpResponseMessage> GetEventAsync(
            this HttpClient client,
            Guid eventId)
        {
            return await client.GetAsync($"/events/{eventId}");
        }
    }
}
