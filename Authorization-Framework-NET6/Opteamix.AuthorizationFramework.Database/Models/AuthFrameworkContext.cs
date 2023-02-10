using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Opteamix.AuthorizationFramework.LogService.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Opteamix.AuthorizationFramework.Data.Model
{
    public partial class AuthFrameworkContext : DbContext
    {

        public AuthFrameworkContext()
        {
        }

        public AuthFrameworkContext(DbContextOptions<AuthFrameworkContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        public virtual DbSet<Application> Applications { get; set; }
        public virtual DbSet<Audit> Audits { get; set; }
        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<Entity> Entities { get; set; }
        public virtual DbSet<EntityPrivilege> EntityPrivileges { get; set; }
        public virtual DbSet<Module> Modules { get; set; }
        public virtual DbSet<Privilege> Privileges { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<RoleEntity> RoleEntities { get; set; }
        public virtual DbSet<EntityRolePrivilege> EntityRolePrivileges { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<UserEntity> UserEntity { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<EntityRolePrivilege>(
                    eb =>
                    {
                        eb.HasNoKey();
                        eb.ToView("vw_EntityRolePrivilege");
                    });
        }

        public async override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries().Where(x => x.State == EntityState.Added || x.State == EntityState.Modified
            || x.State == EntityState.Deleted);
            List<Audit> auditList = new List<Audit>();

            //foreach (var entityEntry in entries)
            //{
            //    if (entityEntry.State == EntityState.Added || entityEntry.State == EntityState.Modified
            //        || entityEntry.State == EntityState.Deleted)
            //    {
            //        var audit = new Audit
            //        {
            //            Action = entityEntry.State.ToString(),
            //            Data = JsonConvert.SerializeObject(entityEntry.Entity),
            //            Entity = entityEntry.Metadata.ShortName(),
            //            CreatedDate = DateTime.Now
            //        };
            //        auditList.Add(audit);
            //    }
            //}

            foreach (var entityEntry in entries)
            {
                AuditData(ref auditList, entityEntry);
            }

            Audits.AddRange(auditList);

            return await base.SaveChangesAsync();
        }
        private void AuditData(ref List<Audit> auditList, EntityEntry entityEntry)
        {
            Dictionary<string, string> currentValue = new Dictionary<string, string>();

            foreach (var prop in entityEntry.Properties)
            {
                currentValue.Add(prop.Metadata.Name, prop.CurrentValue?.ToString());
            }
            var audit = new Audit
            {
                Action = entityEntry.State.ToString(),
                Entity = entityEntry.Metadata.ShortName(),
                Data = JsonConvert.SerializeObject(currentValue),
                CreatedDate = DateTime.Now
            };

            auditList.Add(audit);
        }
    }
}
