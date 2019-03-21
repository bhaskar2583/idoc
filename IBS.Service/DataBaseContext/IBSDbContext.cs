using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using IBS.Core.Entities;

namespace IBS.Service.DataBaseContext
{
    public class IBSDbContext : DbContext
    {
        public IBSDbContext():base()
        {
            Database.SetInitializer<IBSDbContext>(null);
        }

        public virtual DbSet<Carrier> Carriers { get; set; }
        public virtual DbSet<Policy> Policies { get; set; }
        public virtual DbSet<Hospital> Hospitals { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBinder)
        {
            modelBinder.Entity<Carrier>().Map(entity =>
            {
                entity.Property(p => p.Id);
                entity.Property(p => p.IsActive);
                entity.Property(p => p.Name);
                entity.Property(p => p.Email);
                entity.Property(p => p.Phone);
                entity.Property(p => p.Address);
                entity.Property(p => p.AddUser);
                entity.Property(p => p.AddDate);
                entity.Property(p => p.RevUser);
                entity.Property(p => p.RevDate);

                entity.ToTable("carriers");
            });
            modelBinder.Entity<Policy>().Map(entity =>
            {
                entity.Property(p => p.Id);
                entity.Property(p => p.Active);
                entity.Property(p => p.Name);

                entity.ToTable("policies");
            });
            modelBinder.Entity<Hospital>().Map(entity =>
            {
                entity.Property(p => p.Id);
                entity.Property(p => p.Active);
                entity.Property(p => p.Name);

                entity.ToTable("hospitals");
            });
        }

    }
}
