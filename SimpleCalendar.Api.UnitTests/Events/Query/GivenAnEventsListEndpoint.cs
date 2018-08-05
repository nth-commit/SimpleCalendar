using Newtonsoft.Json;
using SimpleCalendar.Api.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SimpleCalendar.Api.UnitTests.Events.Query
{
    public abstract class GivenAnEventsListEndpoint : GivenAnyContext
    {
        protected const string Level1RegionId = GivenAnyContextRegionExtensions.Level1RegionId;
        protected const string Level2RegionId = GivenAnyContextRegionExtensions.Level2RegionId;
        protected const string Level3RegionId = GivenAnyContextRegionExtensions.Level3RegionId;

        protected const string User1Id = "User1";
        protected const string User2Id = "User2";
        protected const string AdministratorId = "Administrator";

        protected class EventDefinition
        {
            public bool IsDeleted { get; set; } = false;

            public bool IsPublished { get; set; } = true;

            public bool IsPublic { get; set; } = true;

            public string RegionId { get; set; }

            public string Name { get; set; }

            public string CreatedById { get; set; }

            public DateTime StartTime { get; set; }

            public DateTime EndTime { get; set; }
        }

        protected abstract IEnumerable<EventDefinition> EventDefinitions { get; }

        protected IEnumerable<EventDefinition> DeletedEvents => EventDefinitions.Where(e => e.IsDeleted);

        protected IEnumerable<EventDefinition> NonDeletedEvents => EventDefinitions.Where(e => !e.IsDeleted);

        public GivenAnEventsListEndpoint()
        {
            InitializeAsync().GetAwaiter().GetResult();
        }

        private async Task InitializeAsync()
        {
            await this.GivenARegionHierarchyAsync();

            foreach (var eventDefinition in EventDefinitions)
            {
                var region = await this.GetAGivenRegionById(eventDefinition.RegionId);

                await this.GivenAnEventAsync(new EventEntity()
                {
                    RegionId = region.Id,
                    IsDeleted = eventDefinition.IsDeleted,
                    IsPublished = eventDefinition.IsPublished,
                    IsPublic = eventDefinition.IsPublic,
                    CreatedByEmail = eventDefinition.CreatedById,
                    DataJson = JsonConvert.SerializeObject(new
                    {
                        Name = eventDefinition.Name
                    }),
                    DataJsonVersion = 1,
                    StartTime = eventDefinition.StartTime,
                    EndTime = eventDefinition.EndTime
                });
            }
        }
    }
}
