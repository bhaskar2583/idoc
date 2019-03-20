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
