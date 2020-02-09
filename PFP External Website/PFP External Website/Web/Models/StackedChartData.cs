using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Models
{
    public class StackedChartData
    {
        public string[] labels { get; set; }

        public dataset[] datasets { get; set; }

        public string[] ChartAllColors { get; set; }
    }

    public class dataset
    {
        public string label { get; set; }

        public string backgroundColor { get; set; }

        public string borderColor { get; set; }

        public bool fill { get; set; }

        public int lineTension { get; set; }

        public decimal[] data { get; set; }

        public string[] ChartAllLabels { get; set; }

        public int maxValue { get; set; }

        public string Quarter { get; set; }
    }

    public class MLookup
    {
        public string ValueSet { get; set; }
        public string Category { get; set; }
        public bool? Flag { get; set; }
    }

    public class ReportVM
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public string MONTHTEXT { get; set; }
        public decimal NUMERATOR { get; set; }
        public decimal DENOMINATOR { get; set; }
        public decimal PFPRATE { get; set; }
        public decimal HOSPRATE { get; set; }
        public int HOSCOUNT { get; set; }
    }
}