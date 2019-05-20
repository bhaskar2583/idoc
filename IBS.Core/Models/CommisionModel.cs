using IBS.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBS.Core.Models
{
    public class CommisionModel : BaseModel
    {
        public int Id { get; set; }
        public int CarrierId { get; set; }
        public string CarrierName { get; set; }
        public int PolicyId { get; set; }
        public string PolicyNumber { get; set; }
        public int ClientId { get; set; }
        public string CleintName { get; set; }
        public int DivisionId { get; set; }
        public string DivisionName { get; set; }
        public int ClientPolicyId { get; set; }
        public decimal? CommisionValue { get; set; }
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? AppliedDate { get; set; }
        public string AppliedDateAsString { get; set; }
        public string PaymentId { get; set; }
        public DateTime? StatementDate { get; set; }
        public string StatementDateAsString { get; set; }
        public int CoverageId { get; set; }
        public string CoverageName { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string Status { get; set; }

        public List<Coverage> Coverages { get; set; }
        public Coverage SelectedCoverage { get; set; }
        public List<Product> Products { get; set; }
        public List<CorporateProduct> CorporateProducts { get; set; }
        public Product SelectedProduct { get; set; }
        public CorporateProduct SelectedCorporateProduct { get; set; }

        public DateTime? ReconcilationPaymentDate { get; set; }
        public string ReconcilationPaymentDateAsString { get; set; }
        public string ReconsilationStatus{ get; set; }
        public CommisionModel()
        {
            SelectedCoverage = new Coverage();
            Coverages = new List<Coverage>();
            Products = new List<Product>();
            CorporateProducts = new List<CorporateProduct>();
        }
    }
}
