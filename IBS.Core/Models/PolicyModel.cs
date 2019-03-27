using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace IBS.Core.Models
{
    public class PolicyModel : BaseModel
    {
        public int Id { get; set; }
        [Required]
        public string PolicyNumber { get; set; }
        [Required]
        public string PolicyType { get; set; }
        public int CarId { get; set; }

        public List<CarrierDdlModel> Carriers { get; set; }
        [Required]
        public DateTime EffectiveDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        [Required]
        public bool IsGroupInsurance { get; set; }
        public bool IsActive{ get; set; }
        public PolicyModel()
        {
            Carriers = new List<CarrierDdlModel>();
        }

    }
}
