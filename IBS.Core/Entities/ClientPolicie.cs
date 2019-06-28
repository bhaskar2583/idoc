using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBS.Core.Entities
{
    public class ClientPolicie : BaseEntity
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public int PolicieId { get; set; }
        public bool IsActive { get; set; }
    }
}
