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

        public static List<SelectListForYears> GetPreviousYearsSelectList(int prefix)
        {
            List<SelectListForYears> years = new List<SelectListForYears>();
            int currentYear = DateTime.Now.Year;

            for (int i = 0; i < prefix; i++)
            {
                years.Add(new SelectListForYears()
                {
                    Id = currentYear - i,
                    Name = Convert.ToString(currentYear - i)
                });
            }
            return years;
        }

        public static List<string> GetMonths()
        {
            return new List<string>() { "jan", "feb", "mar", "apr", "jun", "jul", "aug", "sep", "oct", "nov", "dec"};
        }
    }
}
