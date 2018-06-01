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
            modelBuilder.Entity<RegionEntity>().HasData(new RegionEntity()
            {
                Id = Constants.RootRegionId
            });

            modelBuilder.Entity<RegionRoleEntity>().HasAlternateKey(nameof(RegionRoleEntity.RegionId), nameof(RegionRoleEntity.UserId));
            modelBuilder.Entity<RegionRoleEntity>().HasData(new RegionRoleEntity()
            {
                Id = "ROOT_ADMIN",
                RegionId = Constants.RootRegionId,
                UserId = "google-oauth2|103074202427969604113",
                Role = Framework.Identity.Role.Administrator | Framework.Identity.Role.User
            });

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<RegionEntity> Regions { get; set; }

        public DbSet<RegionRoleEntity> RegionRoles { get; set; }

        public DbSet<EventEntity> Events { get; set; }
    }
}
