using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBS.Core.Entities
{
    public class Client
    {
        public int Id { get; set; }

        public int Hospital_Id { get; set; }

        public int Carrier_Id { get; set; }

        public int Policy_Id { get; set; }
    }
}
