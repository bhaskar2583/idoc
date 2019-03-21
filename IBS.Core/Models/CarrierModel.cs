using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBS.Core.Models
{
    public class CarrierModel: BaseModel
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public bool IsActive { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
    }
}
