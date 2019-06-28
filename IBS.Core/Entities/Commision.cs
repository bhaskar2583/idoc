using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBS.Core.Entities
{
    public class Commision : BaseEntity
    {
        public int Id { get; set; }
        public int CarrierId { get; set; }
        public int PolicyId { get; set; }
        public int ClientId { get; set; }
        public int ClientPolicyId { get; set; }
        public decimal? CommisionValue { get; set; }
        public DateTime? AppliedDate { get; set; }
        public string PaymentId { get; set; }
        public DateTime? StatementDate { get; set; }
        public object Status { get; set; }
        public DateTime? ReconcilationDate { get; set; }
        public string ReconcilationStatus { get; set; }
    }
}