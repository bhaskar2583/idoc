using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBS.Core.Models
{
    public class AddPolicyBudget
    {
        public int Id { get; set; }
        public int ClientPolicyId { get; set; }
        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public int PolicyId { get; set; }
        public string PolicyNumber { get; set; }
        public string Coverage { get; set; }
        public string Product { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Please enter year")]
        public int Year { get; set; }
        [Required]
        public decimal JanBudget { get; set; }
        public decimal FebBudget { get; set; }
        public decimal MarchBudget { get; set; }
        public decimal AprilBudget { get; set; }
        public decimal MayBudget { get; set; }
        public decimal JunBudget { get; set; }
        public decimal JulyBudget { get; set; }
        public decimal AugBudget { get; set; }
        public decimal SepBudget { get; set; }
        public decimal OctBudget { get; set; }
        public decimal NovBudget { get; set; }
        public decimal DecBudget { get; set; }
        public decimal TotalBudget { get; set; }
    }
    public class PolicyBudgetsModel
    {
        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public int PolicyId { get; set; }
        public string PolicyNumber { get; set; }
        public int Year { get; set; }
        public decimal JanBudget { get; set; }
        public decimal FebBudget { get; set; }
        public decimal MarchBudget { get; set; }
        public decimal AprilBudget { get; set; }
        public decimal MayBudget { get; set; }
        public decimal JunBudget { get; set; }
        public decimal JulyBudget { get; set; }
        public decimal AugBudget { get; set; }
        public decimal SepBudget { get; set; }
        public decimal OctBudget { get; set; }
        public decimal NovBudget { get; set; }
        public decimal DecBudget { get; set; }
        public decimal TotalBudget { get; set; }
    }
}