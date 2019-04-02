using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBS.Core.Entities
{
    public class PolicieBudget: BaseEntity
    {
        public int Id { get; set; }
        public int PolicyId { get; set; }
        public string BudgetKey { get; set; }
        public int BudgetValue { get; set; }
        public bool IsActive { get; set; }
    }
}
