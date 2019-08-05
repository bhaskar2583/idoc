using IBS.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBS.Core.Models
{
    public class ExceptionCommissionModel : BaseModel
    {
        public int Id { get; set; }
        public int CarrierId { get; set; }
        public int PolicyId { get; set; }
        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public int ClientPolicyId { get; set; }
        public decimal? CommissionValue { get; set; }

        public string CommissionString { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? AppliedDate { get; set; }
        public string AppliedDateAsString { get; set; }

        public string PaymentId { get; set; }
        public DateTime? StatementDate { get; set; }

        public string StatementDateAsString { get; set; }
        public DateTime? LoadDate { get; set; }
        public string ProductType { get; set; }
        public string CoverageType { get; set; }
        public string PolicyNumber { get; set; }


        public List<Coverage> Coverages { get; set; }
        public Coverage SelectedCoverage { get; set; }
        public List<Product> Products { get; set; }
        public List<CorporateProduct> CorporateProducts { get; set; }
        public Product SelectedProduct { get; set; }
        public CorporateProduct SelectedCorporateProduct { get; set; }
        public ExceptionCommissionModel()
        {
            SelectedCoverage = new Coverage();
            Coverages = new List<Coverage>();
            Products = new List<Product>();
            CorporateProducts = new List<CorporateProduct>();
        }
    }

    public class AssignClientToPolicy
    {
        public int Id { get; set; }
        public string PolicyNo { get; set; }
        public int PolicyId { get; set; }
        public int ClinetId { get; set; }
        public List<Client> Clients { get; set; }
        public AssignClientToPolicy()
        {
            Clients = new List<Client>();
        }
    }
         
}
