using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBS.Service.Utils
{
    public class CommonUtil
    {
        public static List<string> GetStatus()
        {
            List<string> Status = new List<string>()
            {
                "Open",
                "Verified"
            };

            return Status;
        }
    }
}