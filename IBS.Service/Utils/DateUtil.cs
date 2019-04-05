using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBS.Service.Utils
{
    public class DateUtil
    {
        public static DateTime GetCurrentDate()
        {
            return DateTime.Now;
        }

        public static List<int> GetPreviousYears(int prefix)
        {
            List<int> years = new List<int>();
            int currentYear = DateTime.Now.Year;

            for (int i = 0; i < prefix; i++)
            {
                years.Add(currentYear - i);
            }
            return years;
        }
    }
}
