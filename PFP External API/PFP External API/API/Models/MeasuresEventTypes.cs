namespace API.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    public class MeasuresEventTypes
    {
        public int EMM_Id { get; set; }
        public string Measure { get; set; }
        public string EventType { get; set; }
        public string TimePeriodType { get; set; }
    }
}
