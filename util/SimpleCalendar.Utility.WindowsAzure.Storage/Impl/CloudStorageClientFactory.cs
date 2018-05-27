using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Table;
using SimpleCalendar.Utility.Configuration;

namespace SimpleCalendar.Utility.WindowsAzure.Storage.Impl
{
    class CloudStorageClientFactory : ICloudStorageClientFactory
    {
        private readonly IConfiguration _configuration;

        public CloudStorageClientFactory(
            IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public CloudBlobClient CreateBlobClient() => GetStorageAccount().CreateCloudBlobClient();

        public CloudTableClient CreateTableClient() => GetStorageAccount().CreateCloudTableClient();

        private CloudStorageAccount GetStorageAccount() => CloudStorageAccount.Parse(_configuration.GetConnectionString(typeof(Marker)));
    }
}
