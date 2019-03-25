using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Threading.Tasks;

namespace IBS.Core.Entities
{
    public class Policie : BaseEntity
    {
        public int Id { get; set; }
        public string PolicyNumber { get; set; }
        public bool? IsActive { get; set; }
        public string PolicyType { get; set; }
        public int CarId { get; set; }
        public DateTime EffectiveDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool? IsGroupInsurance { get; set; }

    }
}
