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

        public Decimal MarA { get; set; }
        public Decimal MarB { get; set; }
        public Decimal MarF { get { return MarA - MarB; } }

        public Decimal MarYA { get { return FebYA + MarA; } }
        public Decimal MarYB { get { return FebYB + MarB; } }
        public Decimal MarYF { get { return FebYF + MarF; } }

        public Decimal AprA { get; set; }
        public Decimal AprB { get; set; }
        public Decimal AprF { get { return AprA - AprB; } }

        public Decimal AprYA { get { return MarYA + AprA; } }
        public Decimal AprYB { get { return MarYB + AprB; } }
        public Decimal AprYF { get { return MarYF + AprF; } }

        public Decimal MayA { get; set; }
        public Decimal MayB { get; set; }
        public Decimal MayF { get { return MayA - MayB; } }

        public Decimal MayYA { get { return AprYA + MayA; } }
        public Decimal MayYB { get { return AprYB + MayB; } }
        public Decimal MayYF { get { return AprYF + MayF; } }


        public Decimal JunA { get; set; }
        public Decimal JunB { get; set; }
        public Decimal JunF { get { return JunA - JunA; } }

        public Decimal JunYA { get { return MayYA + JunA; } }
        public Decimal JunYB { get { return MayYB + JunA; } }
        public Decimal JunYF { get { return MayYF + JunF; } }

        public Decimal JulA { get; set; }
        public Decimal JulB { get; set; }
        public Decimal JulF { get { return JulA - JulB; } }

        public Decimal JulYA { get { return JunYA + JulA; } }
        public Decimal JulYB { get { return JunYB + JulB; } }
        public Decimal JulYF { get { return JunF + JulF; } }


        public Decimal AugA { get; set; }
        public Decimal AugB { get; set; }
        public Decimal AugF { get { return AugA - AugB; } }

        public Decimal AugYA { get { return JulYA + AugA; } }
        public Decimal AugYB { get { return JulYB + AugB; } }
        public Decimal AugYF { get { return JulYF + AugF; } }


        public Decimal SepA { get; set; }
        public Decimal SepB { get; set; }
        public Decimal SepF { get { return SepA - SepB; } }

        public Decimal SepYA { get { return AugYA + SepA; } }
        public Decimal SepYB { get { return AugYB + SepB; } }
        public Decimal SepYF { get { return AugYF + SepF; } }


        public Decimal OctA { get; set; }
        public Decimal OctB { get; set; }
        public Decimal OctF { get { return OctA - OctB; } }

        public Decimal OctYA { get { return SepYA + OctA; } }
        public Decimal OctYB { get { return SepYB + OctB; } }
        public Decimal OctYF { get { return SepYF + OctF; } }

        public Decimal NovA { get; set; }
        public Decimal NovB { get; set; }
        public Decimal NovF { get { return NovA - NovB; } }

        public Decimal NovYA { get { return OctYA + NovA; } }
        public Decimal NovYB { get { return OctYB + NovB; } }
        public Decimal NovYF { get { return OctYF + NovF; } }


        public Decimal DecA { get; set; }
        public Decimal DecB { get; set; }
        public Decimal DecF { get { return DecA - DecB; } }

        public Decimal DecYA { get { return NovYA + DecA; } }
        public Decimal DecYB { get { return NovYB + DecB; } }
        public Decimal DecYF { get { return NovYF + DecF; } }

        public Decimal Total { get; set; }

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
                case 3:
                    summary.MarA = model.CommisionValue ?? 0;
                    break;
                case 4:
                    summary.AprA = model.CommisionValue ?? 0;
                    break;
                case 5:
                    summary.MayA = model.CommisionValue ?? 0;
                    break;
                case 6:
                    summary.JunA = model.CommisionValue ?? 0;
                    break;
                case 7:
                    summary.JulA = model.CommisionValue ?? 0;
                    break;
                case 8:
                    summary.AugA = model.CommisionValue ?? 0;
                    break;
                case 9:
                    summary.SepA = model.CommisionValue ?? 0;
                    break;
                case 10:
                    summary.OctA = model.CommisionValue ?? 0;
                    break;
                case 11:
                    summary.NovA = model.CommisionValue ?? 0;
                    break;
                case 12:
                    summary.DecA = model.CommisionValue ?? 0;
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
                case 3:           
                    summary.MarB = bud.BudgetValue;
                    break;        
                case 4:            
                    summary.AprB = bud.BudgetValue;
                    break;        
                case 5:           
                    summary.MayB = bud.BudgetValue;
                    break;        
                case 6:            
                    summary.JunB = bud.BudgetValue;
                    break;         
                case 7:            
                    summary.JulB = bud.BudgetValue;
                    break;         
                case 8:            
                    summary.AugB = bud.BudgetValue;
                    break;         
                case 9:            
                    summary.SepB = bud.BudgetValue;
                    break;         
                case 10:           
                    summary.OctB = bud.BudgetValue;
                    break;         
                case 11:           
                    summary.NovB = bud.BudgetValue;
                    break;         
                case 12:           
                    summary.DecB = bud.BudgetValue;
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
