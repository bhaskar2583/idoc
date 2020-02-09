using API.Models.EDM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models
{
    public class MeasuresData
    {
        public string MeasuresDataId { get; set; }
        public string Measure { get; set; }
        public string EventType { get; set; }
        public string OrgId { get; set; }
        public string TimePeriodType { get; set; }
        public IEnumerable<Measure> Measures { get; set; }
        public int FromYear { get; set; }
    }

    public class Measure
    {
        public string MeasureId { get; set; }
        public int HosId { get; set; }
        public int CalId { get; set; }
        public int EmmId { get; set; }
        public string MonthYear { get; set; }
        public decimal Numerator { get; set; }
        public decimal Denominator { get; set; }
        public int Multiplier { get; set; }
        public decimal Measurement { get; set; }
        public int? AnalysisPeriodId { get; set; }
        public string SourceType { get; set; }
        public int OrderBy { get; set; }
        public string UpdatedBy { get; set; }
        public List<AnalysisPeriod> AnalysisPeriod { get; set; }
    }

    public class AnalysisPeriod
    {
        public int AnalysisPeriodId { get; set; }
        public string AnalysisPeriodName { get; set; }
    }
}