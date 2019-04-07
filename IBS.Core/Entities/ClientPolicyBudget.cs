using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBS.Core.Entities
{
    
    public class ClientPolicyBudget: BaseEntity
    {
        public int Id { get; set; }
        public int ClientPolicyId { get; set; }
        public int PolicyId { get; set; }
        public int ClientId { get; set; }
        public int BudgetYear { get; set; }
        public string BudgetMonth { get; set; }
        public decimal BudgetValue { get; set; }
        public bool IsActive { get; set; }
    }
}
