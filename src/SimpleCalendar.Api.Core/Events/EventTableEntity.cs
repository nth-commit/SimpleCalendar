using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleCalendar.Api.Core.Events
{
    public class EventTableEntity : TableEntity
    {
        public string DataJson { get; set; }

        public int DataJsonVersion { get; set; } = 1;
    }
}
