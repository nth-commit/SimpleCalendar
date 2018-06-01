using AutoMapper;
using Microsoft.WindowsAzure.Storage.Table;
using SimpleCalendar.Utility.WindowsAzure.Storage;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCalendar.Api.Core.Events
{
    public class TableStorageEventRepository : IEventRepository
    {
        private readonly ICloudStorageClientFactory _cloudStorageClientFactory;
        private readonly IMapper _mapper;

        public TableStorageEventRepository(
            ICloudStorageClientFactory cloudStorageClientFactory,
            IMapper mapper)
        {
            _cloudStorageClientFactory = cloudStorageClientFactory;
            _mapper = mapper;
        }

        public async Task<Event> CreateEventAsync(Event ev)
        {
            var eventTableEntity = _mapper.Map<EventTableEntity>(ev);

            var table = await GetTableReferenceAsync();

            await table.ExecuteAsync(TableOperation.Insert(eventTableEntity));

            return ev;
        }

        private async Task<CloudTable> GetTableReferenceAsync()
        {
            var tableClient = _cloudStorageClientFactory.CreateTableClient();

            var table = tableClient.GetTableReference("events");
            await table.CreateIfNotExistsAsync();

            return table;
        }
    }
}
