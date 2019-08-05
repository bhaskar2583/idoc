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

        public virtual DbSet<Coverage> Coverages { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ClientPolicie> ClientPolicies { get; set; }
        public virtual DbSet<ClientPolicyBudget> ClientPolicieBudgets { get; set; }
        public virtual DbSet<Commision> Commisions { get; set; }

        public virtual DbSet<CorporateProduct> CorporateProduct { get; set; }
        public virtual DbSet<CorporateProductsXProduct> CorporateProductsXProducts { get; set; }
        public virtual DbSet<InvalidCommission> InvalidCommissions { get; set; }
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
                entity.Property(p => p.SfiId).HasColumnName("Car_SlaesforceId");

                entity.ToTable("carrier");
            });
            modelBinder.Entity<Policie>().Map(entity =>
            {
                entity.Property(p => p.Id).HasColumnName("Pol_Id");
                entity.Property(p => p.IsActive).HasColumnName("Pol_IsActive");
                entity.Property(p => p.PolicyNumber).HasColumnName("Pol_PolicyNumber");
                entity.Property(p => p.EffectiveDate).HasColumnName("Pol_EffectiveDate");
                entity.Property(p => p.EndDate).HasColumnName("Pol_EndDate");
                entity.Property(p => p.CarId).HasColumnName("Pol_Car_Id");
                entity.Property(p => p.IsGroupInsurance).HasColumnName("Pol_GroupInsurence");
                entity.Property(p => p.AddUser).HasColumnName("Pol_AddUser");
                entity.Property(p => p.AddDate).HasColumnName("Pol_AddDate");
                entity.Property(p => p.RevUser).HasColumnName("Pol_RevUser");
                entity.Property(p => p.RevDate).HasColumnName("Pol_RevDate");
                entity.Property(p => p.CoverageId).HasColumnName("Pol_Cov_Id");
                entity.Property(p => p.ProductId).HasColumnName("Pol_Pro_Id");

                entity.ToTable("Policy");
            });
            modelBinder.Entity<Client>().Map(entity =>
            {
                entity.Property(p => p.Id).HasColumnName("Cli_Id");
                entity.Property(p => p.IsActive).HasColumnName("Cli_IsActive");
                entity.Property(p => p.SfiId).HasColumnName("Cli_SalesforceId");
                entity.Property(p => p.Division).HasColumnName("Cli_Division");
                entity.Property(p => p.Name).HasColumnName("Cli_Name");
                entity.Property(p => p.AddUser).HasColumnName("Cli_AddUser");
                entity.Property(p => p.AddDate).HasColumnName("Cli_AddDate");
                entity.Property(p => p.RevUser).HasColumnName("Cli_RevUser");
                entity.Property(p => p.RevDate).HasColumnName("Cli_RevDate");

                entity.ToTable("Client");
            });
            modelBinder.Entity<Coverage>().Map(entity =>
            {
                entity.Property(p => p.Id).HasColumnName("Cov_Id");
                entity.Property(p => p.Name).HasColumnName("Cov_Name");
                entity.ToTable("Coverage");
            });
            modelBinder.Entity<Product>().Map(entity =>
            {
                entity.Property(p => p.Id).HasColumnName("Pro_Id");
                entity.Property(p => p.Name).HasColumnName("Pro_Name");
                entity.Property(p => p.CoverageId).HasColumnName("Pro_Cov_Id");
                entity.ToTable("Product");
            });
            modelBinder.Entity<ClientPolicie>().Map(entity =>
            {
                entity.Property(p => p.Id).HasColumnName("Cp_Id");
                entity.Property(p => p.IsActive).HasColumnName("Cp_IsActive");
                entity.Property(p => p.ClientId).HasColumnName("Cp_ClientId");
                entity.Property(p => p.PolicieId).HasColumnName("Cp_PolicieId");
                entity.Property(p => p.AddUser).HasColumnName("Cp_AddUser");
                entity.Property(p => p.AddDate).HasColumnName("Cp_AddDate");
                entity.Property(p => p.RevUser).HasColumnName("Cp_RevUser");
                entity.Property(p => p.RevDate).HasColumnName("Cp_RevDate");

                entity.ToTable("ClientPolicy");
            });
            modelBinder.Entity<ClientPolicyBudget>().Map(entity =>
            {
                entity.Property(p => p.Id).HasColumnName("Pb_Id");
                entity.Property(p => p.IsActive).HasColumnName("Pb_IsActive");
                entity.Property(p => p.ClientPolicyId).HasColumnName("Pb_Cpl_Id");
                entity.Property(p => p.ClientId).HasColumnName("Pb_Cli_Id");
                entity.Property(p => p.PolicyId).HasColumnName("Pb_Pol_Id");
                entity.Property(p => p.BudgetYear).HasColumnName("Pb_BudgetYear");
                entity.Property(p => p.BudgetMonth).HasColumnName("Pb_BudgetMonth");
                entity.Property(p => p.BudgetValue).HasColumnName("Pb_BudgetValue");
                entity.Property(p => p.AddUser).HasColumnName("Pb_AddUser");
                entity.Property(p => p.AddDate).HasColumnName("Pb_AddDate");
                entity.Property(p => p.RevUser).HasColumnName("Pb_RevUser");
                entity.Property(p => p.RevDate).HasColumnName("Pb_RevDate");

                entity.ToTable("ClientPolicyBudget");
            });

            modelBinder.Entity<Commision>().Map(entity =>
            {
                entity.Property(p => p.Id).HasColumnName("Com_Id");
                entity.Property(p => p.CarrierId).HasColumnName("Com_CarrierId");
                entity.Property(p => p.PolicyId).HasColumnName("Com_PolicyId");
                entity.Property(p => p.ClientId).HasColumnName("Com_ClientId");
                entity.Property(p => p.ClientPolicyId).HasColumnName("Com_ClientPolicyId");
                entity.Property(p => p.CommisionValue).HasColumnName("Com_Commision");
                entity.Property(p => p.AppliedDate).HasColumnName("Com_AppliedDate");
                entity.Property(p => p.PaymentId).HasColumnName("Com_PaymentId");
                entity.Property(p => p.StatementDate).HasColumnName("Com_StatementDate");
                entity.Property(p => p.AddUser).HasColumnName("Com_AddUser");
                entity.Property(p => p.AddDate).HasColumnName("Com_AddDate");
                entity.Property(p => p.RevUser).HasColumnName("Com_RevUser");
                entity.Property(p => p.RevDate).HasColumnName("Com_RevDate");
                entity.Property(p => p.ReconcilationDate).HasColumnName("Com_ReconcilationDate");
                entity.Property(p => p.ReconcilationStatus).HasColumnName("Com_ReconcilationStatus");
                entity.Property(p => p.CommisionString).HasColumnName("Com_CommisionString");
                entity.Property(p => p.IsExceptionCommission).HasColumnName("Com_IsExceptionCommission");

                entity.ToTable("Commission");
            });

            modelBinder.Entity<CorporateProduct>().Map(entity =>
            {
                entity.Property(p => p.Id).HasColumnName("Cp_Id");
                entity.Property(p => p.Name).HasColumnName("Cp_Name");
                entity.Property(p => p.CoverageId).HasColumnName("Cp_Cov_Id");
                entity.ToTable("CFProduct");
            });

            modelBinder.Entity<CorporateProductsXProduct>().Map(entity =>
            {
                entity.Property(p => p.Id).HasColumnName("Id");
                entity.Property(p => p.CorporateProductId).HasColumnName("Cop_Pro_Id");
                entity.Property(p => p.ProductId).HasColumnName("Pro_Id");
                entity.ToTable("CFProductXProduct");
            });

            modelBinder.Entity<InvalidCommission>().Map(entity =>
            {
                entity.Property(p => p.Id).HasColumnName("Com_Id");
                entity.Property(p => p.CarrierId).HasColumnName("Com_CarrierId");
                entity.Property(p => p.PolicyId).HasColumnName("Com_PolicyId");
                entity.Property(p => p.ClientId).HasColumnName("Com_ClientId");

                entity.Property(p => p.PolicyNumber).HasColumnName("Com_PolicyNumber");
                entity.Property(p => p.ClientPolicyId).HasColumnName("Com_ClientPolicyId");
                entity.Property(p => p.CommissionValue).HasColumnName("Com_Commission");
                entity.Property(p => p.AppliedDate).HasColumnName("Com_AppliedDate");

                entity.Property(p => p.StatementDate).HasColumnName("Com_StatementDate");
                entity.Property(p => p.PaymentId).HasColumnName("Com_PaymentId");

                entity.Property(p => p.ProductType).HasColumnName("Com_ProductType");
                entity.Property(p => p.CoverageType).HasColumnName("Com_CoverageType");
                entity.Property(p => p.IsDumped).HasColumnName("Com_IsDumped");
                entity.Property(p => p.AddUser).HasColumnName("Com_AddUser");
                entity.Property(p => p.AddDate).HasColumnName("Com_AddDate");
                entity.Property(p => p.RevUser).HasColumnName("Com_RevUser");
                entity.Property(p => p.RevDate).HasColumnName("Com_RevDate");

                entity.ToTable("InvalidCommission");
            });
        }

    }
}