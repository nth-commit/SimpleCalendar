using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCalendar.Api.Core.Data
{
    public class CoreDbContext : DbContext
    {
        public CoreDbContext(DbContextOptions<CoreDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RegionRoleEntity>()
                .HasAlternateKey(nameof(RegionRoleEntity.RegionId), nameof(RegionRoleEntity.UserId));

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<RegionEntity> Regions { get; set; }

        public DbSet<RegionRoleEntity> RegionRoles { get; set; }

        public DbSet<EventEntity> Events { get; set; }
    }
}
