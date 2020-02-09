namespace Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web;

    public class MeasuresEventTypes
    {
        public int EMM_Id { get; set; }

        [Display(Name = "Measure")]
        public string Measure { get; set; }

        [Display(Name = "Event Type")]
        public string EventType { get; set; }

        [Display(Name = "Time Period Type")]
        public string TimePeriodType { get; set; }
    }
}
