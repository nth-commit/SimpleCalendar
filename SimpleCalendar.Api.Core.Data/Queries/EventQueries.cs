﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCalendar.Api.Core.Data
{
    public static class EventQueries
    {
        public static async Task<EventEntity> GetEventByIdAsync(
            this CoreDbContext coreDbContext,
            string id)
        {
            var ev = await coreDbContext.Events.FindAsync(id);

            if (ev != null)
            {
                ev.Region = await coreDbContext.GetRegionByIdAsync(ev.RegionId);
            }

            return ev;
        }

        public static IQueryable<EventEntity> IncludeRegionHierarchy(
            this IQueryable<EventEntity> events,
            bool includeRoles = true)
        {
            // HACK!
            return events
                .Include(e => e.Region)
                    .ThenInclude(r => r.Memberships)
                .Include(e => e.Region)
                    .ThenInclude(r => r.Parent)
                    .ThenInclude(r => r.Memberships)
                .Include(e => e.Region)
                    .ThenInclude(r => r.Parent)
                    .ThenInclude(r => r.Parent)
                    .ThenInclude(r => r.Memberships)
                .Include(e => e.Region)
                    .ThenInclude(r => r.Parent)
                    .ThenInclude(r => r.Parent)
                    .ThenInclude(r => r.Parent)
                    .ThenInclude(r => r.Memberships)
                .Include(e => e.Region)
                    .ThenInclude(r => r.Parent)
                    .ThenInclude(r => r.Parent)
                    .ThenInclude(r => r.Parent)
                    .ThenInclude(r => r.Parent)
                    .ThenInclude(r => r.Memberships);
        }
    }
}
