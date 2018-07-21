using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SimpleCalendar.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCalendar.Api.Core.Data
{
    public class CoreDbContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public CoreDbContext(
            IConfiguration configuration,
            DbContextOptions<CoreDbContext> options) : base(options)
        {
            _configuration = configuration;
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_configuration.GetConnectionString(typeof(CoreDbContext)));
            }

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RegionEntity>().HasData(new RegionEntity()
            {
                Id = Constants.RootRegionId
            });

            modelBuilder.Entity<RegionRoleEntity>().HasData(
                new RegionRoleEntity()
                {
                    Id = Constants.RegionRoles.SuperAdministrator,
                    Name = "Super Admins",
                    Permissions = RegionPermission.All
                },
                new RegionRoleEntity()
                {
                    Id = Constants.RegionRoles.Administrator,
                    Name = "Admins",
                    Permissions = RegionPermission.Events_All | RegionPermission.Memberships_WriteReader | RegionPermission.Memberships_Read,
                    ChildPermissions = RegionPermission.Memberships_WriteWriter
                },
                new RegionRoleEntity()
                {
                    Id = Constants.RegionRoles.User,
                    Name = "Users",
                    Permissions =
                        RegionPermission.Events_Read |
                        RegionPermission.Events_WriteDraft
                });

            modelBuilder.Entity<UserEntity>().HasData(new UserEntity()
            {
                Email = "michaelfry2002@gmail.com",
                ClaimsBySubJson = JsonConvert.SerializeObject(new Dictionary<string, Dictionary<string, string>>()),
                ClaimsBySubVersion = 1
            });

            modelBuilder.Entity<RegionMembershipEntity>().HasAlternateKey(nameof(RegionMembershipEntity.RegionId), nameof(RegionMembershipEntity.UserEmail));
            modelBuilder.Entity<RegionMembershipEntity>().HasData(new RegionMembershipEntity()
            {
                Id = Guid.NewGuid().ToString(),
                RegionId = Constants.RootRegionId,
                UserEmail = "michaelfry2002@gmail.com",
                RegionRoleId = Constants.RegionRoles.SuperAdministrator
            });

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<UserEntity> Users { get; set; }

        public DbSet<RegionEntity> Regions { get; set; }

        public DbSet<RegionRoleEntity> RegionRoles { get; set; }

        public DbSet<RegionMembershipEntity> RegionMemberships { get; set; }

        public DbSet<EventEntity> Events { get; set; }
    }
}
