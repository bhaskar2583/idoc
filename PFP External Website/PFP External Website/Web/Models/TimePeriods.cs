namespace Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    public class Periods
    {
        public string PeriodId { get; set; }
        public string PeriodText { get; set; }
    }

    public static class TimePeriods
    {
        public static List<Periods> GetTimePeriods()
        {
            return new List<Periods>
            {
                new Periods
                {
                    PeriodId = "mq1",
                    PeriodText = " Monthly ( Jan - Mar ) "
                },
                new Periods
                {
                    PeriodId = "mq2",
                    PeriodText = " Monthly ( Apr - Jun ) "
                },
                new Periods
                {
                    PeriodId = "mq3",
                    PeriodText = " Monthly ( Jul - Sep ) "
                },
                new Periods
                {
                    PeriodId = "mq4",
                    PeriodText = " Monthly ( Oct - Dec ) "
                },
                new Periods{
                    PeriodId = "q",
                    PeriodText = " Quarterly "
                }
            };
        }
    }
}
