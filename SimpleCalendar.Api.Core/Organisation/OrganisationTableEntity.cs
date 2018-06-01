using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleCalendar.Api.Core.Organisation
{
    public class OrganisationTableEntity : TableEntity
    {
        public string Name { get; set; }

        public IEnumerable<string> Regions { get; set; }

        public IEnumerable<string> Members { get; set; }

        public OrganisationTableEntity(Organisation organisation)
        {
            var parentRegions = organisation.Regions
                .Select(r => r.Split('.').First())
                .Distinct();

            if (parentRegions.Count() == 0)
            {
                PartitionKey = Core.Regions.Constants.RootRegionId;
            }
            else if (parentRegions.Count() == 1)
            {
                PartitionKey = parentRegions.First();
            }
            else
            {
                PartitionKey = Core.Regions.Constants.RootRegionId;
            }

            RowKey = organisation.Id;
        }
    }
}
