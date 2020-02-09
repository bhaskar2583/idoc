namespace API.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    public class EventMeasureData
    {
        public int EventDataId { get; set; }
        public string EventType { get; set; }
        public string MeasureName { get; set; }
        public int OrderBy { get; set; }
        public List<MeasurementData> MeasurementDatas { get; set; }
    }

    public class MeasurementData
    {
        public string MeasurementDataId { get; set; }        
        public int HosId { get; set; }
        public int CalId { get; set; }
        public int EmmId { get; set; }
        public string MonthYear { get; set; }
        public decimal Numerator { get; set; }
        public decimal Denominator { get; set; }
        public int Multiplier { get; set; }
        public decimal Measurement { get; set; }
        public int OrderBy { get; set; }
        public string UpdatedBy { get; set; }
    }
}
