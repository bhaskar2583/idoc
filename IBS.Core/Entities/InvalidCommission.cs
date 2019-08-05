using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBS.Core.Entities
{
    public class InvalidCommission : BaseEntity
    {
        public int Id { get; set; }
        public int CarrierId { get; set; }
        public int PolicyId { get; set; }
        public int ClientId { get; set; }
        public string PolicyNumber { get; set; }
        public int ClientPolicyId { get; set; }
        public decimal? CommissionValue { get; set; }
        public DateTime? AppliedDate { get; set; }
        public DateTime? StatementDate { get; set; }
        public string PaymentId { get; set; }
        public string ProductType { get; set; }
        public string CoverageType { get; set; }
        public bool IsDumped { get; set; }
    }
}
