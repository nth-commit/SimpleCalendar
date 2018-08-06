using Newtonsoft.Json;
using SimpleCalendar.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCalendar.Api.UnitTests.Events
{
    public static class HttpClientExtensions
    {
        public static async Task<HttpResponseMessage> CreateEventAsync(
            this HttpClient client,
            EventInput create)
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

        public static async Task<IEnumerable<EventOutput>> QueryEventsAndAssertOK(
            this HttpClient client,
            string regionId = Constants.RootRegionId,
            DateTime? fromDate = null,
            DateTime? toDate = null)
        {
            var response = await client.GetQueryEventsResponseAsync(
                regionId: regionId,
                fromDate: fromDate,
                toDate: toDate);

            response.AssertStatusCodeOK();

            return await response.DeserializeEventsAsync();
        }

        public static async Task<IEnumerable<EventOutput>> QueryEventsTodayAndAssertOK(
            this HttpClient client,
            string regionId = Constants.RootRegionId,
            string timezone = null)
        {
            var response = await client.GetQueryEventsTodayResponseAsync(
                regionId: regionId,
                timezone: timezone);

            response.AssertStatusCodeOK();

            return await response.DeserializeEventsAsync();
        }

        public static async Task<HttpResponseMessage> GetQueryEventsResponseAsync(
            this HttpClient client,
            string regionId = null,
            DateTime? fromDate = null,
            DateTime? toDate = null)
        {
            var query = new Dictionary<string, string>();

            if (!string.IsNullOrEmpty(regionId))
            {
                query.Add("regionId", regionId);
            }

            if (fromDate.HasValue)
            {
                query.Add("from", fromDate.Value.ToString("s"));
            }

            if (toDate.HasValue)
            {
                query.Add("to", toDate.Value.ToString("s"));
            }

            return await client.DoQueryAsync("/events", query);
        }

        public static async Task<HttpResponseMessage> GetQueryEventsTodayResponseAsync(
            this HttpClient client,
            string regionId = null,
            string timezone = null)
        {
            var query = new Dictionary<string, string>();

            if (!string.IsNullOrEmpty(regionId))
            {
                query.Add("regionId", regionId);
            }

            if (!string.IsNullOrEmpty(timezone))
            {
                query.Add("timezone", timezone);
            }

            return await client.DoQueryAsync("/events/today", query);
        }

        private static async Task<HttpResponseMessage> DoQueryAsync(
            this HttpClient client,
            string path,
            Dictionary<string, string> query)
        {
            if (query.Any())
            {
                path += "?" + string.Join("&", query.Select(kvp => $"{kvp.Key}={kvp.Value}"));
            }
            return await client.GetAsync(path);
        }
    }
}
