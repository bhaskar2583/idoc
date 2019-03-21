using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBS.Core.Entities
{
    public class BaseEntity
    {
        public string AddUser { get; set; }
        public DateTime AddDate { get; set; }
        public string RevUser { get; set; }
        public DateTime? RevDate { get; set; }
    }
}
