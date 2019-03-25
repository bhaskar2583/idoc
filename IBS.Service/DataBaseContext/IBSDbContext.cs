using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using IBS.Core.Entities;

namespace IBS.Service.DataBaseContext
{
    public sealed class SingletonForEF
    {
        SingletonForEF()
        {
        }
        private static readonly object padlock = new object();
        private static IBSDbContext instance = null;
        public static IBSDbContext Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new IBSDbContext();
                    }
                    return instance;
                }
            }
        }
    }
    public class IBSDbContext : DbContext
    {
        public IBSDbContext() : base()
        {
            Database.SetInitializer<IBSDbContext>(null);
        }

        public virtual DbSet<Carrier> Carriers { get; set; }
        public virtual DbSet<Policie> Policies { get; set; }
        public virtual DbSet<Client> Clients { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBinder)
        {
            modelBinder.Entity<Carrier>().Map(entity =>
            {
                entity.Property(p => p.Id).HasColumnName("Car_Id");
                entity.Property(p => p.IsActive).HasColumnName("Car_IsActive");
                entity.Property(p => p.Name).HasColumnName("Car_Name");
                entity.Property(p => p.Email).HasColumnName("Car_Email");
                entity.Property(p => p.Phone).HasColumnName("Car_Phone");
                entity.Property(p => p.AddressLine1).HasColumnName("Car_AddressLine1");
                entity.Property(p => p.AddressLine2).HasColumnName("Car_AddressLine2");
                entity.Property(p => p.City).HasColumnName("Car_City");
                entity.Property(p => p.State).HasColumnName("Car_State");
                entity.Property(p => p.ZipCode).HasColumnName("Car_ZipCode");
                entity.Property(p => p.AddUser).HasColumnName("Car_AddUser");
                entity.Property(p => p.AddDate).HasColumnName("Car_AddDate");
                entity.Property(p => p.RevUser).HasColumnName("Car_RevUser");
                entity.Property(p => p.RevDate).HasColumnName("Car_RevDate");

                entity.ToTable("carriers");
            });
            modelBinder.Entity<Policie>().Map(entity =>
            {
                entity.Property(p => p.Id).HasColumnName("Pol_Id");
                entity.Property(p => p.IsActive).HasColumnName("Pol_IsActive");
                entity.Property(p => p.PolicyNumber).HasColumnName("Pol_PolicyNumber");
                entity.Property(p => p.PolicyType).HasColumnName("Pol_PolicyType");
                entity.Property(p => p.EffectiveDate).HasColumnName("Pol_EffectiveDate");
                entity.Property(p => p.EndDate).HasColumnName("Pol_EndDate");
                entity.Property(p => p.CarId).HasColumnName("Pol_Car_Id");
                entity.Property(p => p.IsGroupInsurance).HasColumnName("Pol_GroupInsurence");
                entity.Property(p => p.AddUser).HasColumnName("Pol_AddUser");
                entity.Property(p => p.AddDate).HasColumnName("Pol_AddDate");
                entity.Property(p => p.RevUser).HasColumnName("Pol_RevUser");
                entity.Property(p => p.RevDate).HasColumnName("Pol_RevDate");

                entity.ToTable("policies");
            });
            modelBinder.Entity<Client>().Map(entity =>
            {
                entity.Property(p => p.Id);
                entity.Property(p => p.IsActive);
                entity.Property(p => p.Division);
                entity.Property(p => p.Name);

                entity.ToTable("hospitals");
            });
        }

    }
}
