namespace Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;

    public class MeasuresData
    {
        public string MeasuresDataId { get; set; }

        [Display(Name = "Measure")]
        public string Measure { get; set; }

        [Display(Name = "Event Type")]
        public string EventType { get; set; }

        [Display(Name = "Org. Id")]
        public string OrgId { get; set; }
                
        [Display(Name = "Time Period Type")]
        public string TimePeriodType { get; set; }

        public List<Measure> Measures { get; set; }

        public int FromYear { get; set; }
    }

    public class Measure
    {
        public string MeasureId { get; set; }

        [Required]
        public int HosId { get; set; }
        [Required]
        public int CalId { get; set; }
        [Required]
        public int EmmId { get; set; }

        [Display(Name = "Month-Year")]
        public string MonthYear { get; set; }

        public decimal Numerator { get; set; }

        public decimal Denominator { get; set; }

        public int Multiplier { get; set; }

        public decimal Measurement { get; set; }

        [Required]
        [Display(Name = "Analysis Period")]        
        public int? AnalysisPeriodId { get; set; }

        public string SourceType { get; set; }

        public int OrderBy { get; set; }

        [Display(Name = "Updated By")]
        public string UpdatedBy { get; set; }

        public IEnumerable<AnalysisPeriod> AnalysisPeriod { get; set; }

        public SelectList AnalysisPeriodSelectListItems { get; set; }
    }

    public class AnalysisPeriod
    {
        public int AnalysisPeriodId { get; set; }
        public string AnalysisPeriodName { get; set; }
    }
}