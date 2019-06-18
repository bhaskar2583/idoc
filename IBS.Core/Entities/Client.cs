using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBS.Core.Entities
{
    public class Client : BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Division { get; set; }
        public bool IsActive { get; set; }
        public string SfiId { get; set; }
    }
}
