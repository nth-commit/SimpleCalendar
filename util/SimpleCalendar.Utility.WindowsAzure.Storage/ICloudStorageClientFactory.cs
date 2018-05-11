using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleCalendar.Utility.WindowsAzure.Storage
{
    public interface ICloudStorageClientFactory
    {
        CloudBlobClient CreateBlobClient();

        CloudTableClient CreateTableClient();
    }
}
