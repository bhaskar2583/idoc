using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBS.Core.Models
{
    public class PolicyModel : BaseModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string PolicyType { get; set; }
        [Required]
        public int CarId { get; set; }
        [Required]
        public DateTime EffectiveDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        [Required]
        public bool IsGroupInsurence { get; set; }
        public bool IsActive { get; set; }
    }
}
