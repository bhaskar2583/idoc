using IBS.Core.Models;
using IBS.Service.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBS.Service.Utils
{

    public class MonthSummary
    {
        public string PolicyNo { get; set; }
        public string CoverageName { get; set; }
        public Decimal JanA { get; set; }
        public Decimal JanB { get; set; }
        public Decimal JanF { get { return JanA - JanB; } }

        public Decimal JanYA { get { return JanA; } }
        public Decimal JanYB { get { return JanB; } }
        public Decimal JanYF { get { return JanF; } }

        public Decimal FebA { get; set; }
        public Decimal FebB { get; set; }
        public Decimal FebF { get { return FebA - FebB; } }

        public Decimal FebYA { get { return JanYA + FebA; } }
        public Decimal FebYB { get { return JanYB + FebB; } }
        public Decimal FebYF { get { return JanYF + FebF; } }

        public Decimal Total { get { return FebYB; } }

    }
    public static class MonthSummaryReportExtensions
    {
        public static void MapMonthlySummaryActual(this CommisionModel model, MonthSummary summary, ICommonService service)
        {
            int month = Convert.ToDateTime(model.AppliedDate).Month;

            switch (month)
            {
                case 1:
                    summary.JanA = model.CommisionValue??0;
                    break;
                case 2:
                    summary.FebA = model.CommisionValue??0;
                    break;
            }
        }
        public static void MapMonthlySummaryBudget(this CommisionModel model, MonthSummary summary, ICommonService service)
        {
            int month = Convert.ToDateTime(model.AppliedDate).Month;
            int year = Convert.ToDateTime(model.AppliedDate).Year;
            var bud = service.GetAllPolicyBudgetsForPolicyYearMonth(model.PolicyId, year, GetMonth(month));
            if (bud == null)
                return;
            switch (month)
            {
                case 1:
                    summary.JanB = bud.BudgetValue;
                    break;
                case 2:
                    summary.FebB = bud.BudgetValue;
                    break;
               
            }
        }

        private static string GetMonth(int MNumber)
        {
            switch (MNumber)
            {
                case 1:
                    return "jan";
                case 2:
                    return "feb";
                case 3:
                    return "mar";
                case 4:
                    return "apr";
                case 5:
                    return "may";
                case 6:
                    return "jan";
                case 7:
                    return "jul";
                case 8:
                    return "aug";
                case 9:
                    return "sep";
                case 10:
                    return "oct";
                case 11:
                    return "nov";
                case 12:
                    return "dec";
            }
            return string.Empty;
        }
    }
}
