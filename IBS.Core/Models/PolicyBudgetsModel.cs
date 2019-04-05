using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBS.Core.Models
{
    public class PolicyBudgetsModel
    {
        public int PolicyId { get; set; }
        public string PolicyNumber { get; set; }
        public int Year { get; set; }
        public int JanBudget { get; set; }
        public int FebBudget { get; set; }
        public int MarchBudget { get; set; }
        public int AprilBudget { get; set; }
        public int MayBudget { get; set; }
        public int JunBudget { get; set; }
        public int JulyBudget { get; set; }
        public int AugBudget { get; set; }
        public int SepBudget { get; set; }
        public int OctBudget { get; set; }
        public int NovBudget { get; set; }
        public int DecBudget { get; set; }
    }
}
