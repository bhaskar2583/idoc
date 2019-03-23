using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBS.Core.Entities
{       
   
    public class Policy : BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PolicyType { get; set; }
        public int CarId { get; set; }
        public DateTime EffectiveDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsGroupInsurence { get; set; }
        public bool IsActive { get; set; }
    }
}
