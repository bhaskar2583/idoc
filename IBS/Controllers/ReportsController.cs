using IBS.Core.Models;
using IBS.Service.Helpers;
using IBS.Service.Services;
using IBS.Service.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IBS.Controllers
{

    public class SearchFiltrsResult
    {
        public int DisplayTillMonth { get; set; }
        public List<GroupResult> Result { get; set; }
        public List<MonthSummary> MonthSummaryResult { get; set; }
    }

    public class GroupResult
    {
        public List<CommisionModel> Result { get; set; }
        public List<ClientCoverageRevenueByMonth> ClientRevenueResultMonth { get; set; }
        public List<ClientRevenue> ClientRevenueResult { get; set; }

        
    }

    

    public class ClientRevenue
    {
        public List<CommisionModel> Result { get; set; }
    }

    public class ClientCoverageRevenueByMonth
    {
        public string ClientName { get; set; }
        public string CoverageName { get; set; }
        public decimal Jan { get; set; }
        public decimal Feb { get; set; }
        public decimal Mar { get; set; }
        public decimal Apr { get; set; }
        public decimal May { get; set; }
        public decimal Jun { get; set; }
        public decimal July { get; set; }
        public decimal Aug { get; set; }
        public decimal Sep { get; set; }
        public decimal Oct { get; set; }
        public decimal Nov { get; set; }
        public decimal Dec { get; set; }
        public decimal Total { get
            {
                return Jan + Feb + Mar + Apr + May + Jun + July + Aug + Sep + Oct + Nov + Dec;
            } }
    }
    public class SearchFiltrs
    {
        public List<int> DateTypes { get; set; }
        public List<int> Partners { get; set; }
        public List<int> Divisions { get; set; }
        public List<int> Rtype { get; set; }
        public List<int> Git { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
    }
   
   
    public class ReportsController : Controller
    {
        private readonly ICarrierService _carrierService;
        private readonly ICommisionService _commisionService;
        private readonly ICommonService _commonService;
        public ReportsController(ICarrierService carrierService, ICommisionService commisionService, ICommonService commonService)
        {
            _carrierService = carrierService;
            _commisionService = commisionService;
            _commonService = commonService;

        }
        
        // GET: Reports
        public ActionResult Index()
        {
            var commisions = new SearchFiltrsResult();
            return View(commisions);
        }
        [HttpPost]
        public ActionResult Index(SearchFiltrs filters)
        {
             var commisions = new SearchFiltrsResult();

            GetCommissions(filters, out commisions, out List<CommisionModel> data);

            string cId = "";
            var tempRs = new List<GroupResult>();
            var tempR = new GroupResult();
            data.ToList().ForEach(d =>
            {
                if (cId != d.CleintName)
                {
                    tempR = new GroupResult();
                    tempR.Result = new List<CommisionModel>();
                    tempR.Result.Add(d);
                    tempRs.Add(tempR);
                }
                else
                {
                    tempR.Result.Add(d);
                }
                cId = d.CleintName;
            });
            commisions.Result = tempRs;
            //return View(commisions);
            return Json(commisions, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ClientRevenue()
        {
            var commisions = new SearchFiltrsResult();
            return View(commisions);
        }
        [HttpPost]
        public ActionResult ClientRevenueByMonths(SearchFiltrs filters)
        {
            SearchFiltrsResult commisions;
            GetCommissions(filters, out commisions, out List<CommisionModel> data);

            string cId = "";
            var tempRs = new List<GroupResult>();
            var tempR = new GroupResult();
            data.ToList().ForEach(d =>
            {
                if (cId != d.CleintName)
                {
                    tempR = new GroupResult();
                    tempR.Result = new List<CommisionModel>();
                    tempR.Result.Add(d);
                    tempRs.Add(tempR);
                }
                else
                {
                    tempR.Result.Add(d);
                }
                cId = d.CleintName;
            });


            tempRs.ForEach(c =>
            {
                var CM = c.Result.OrderBy(cm => cm.CoverageId).ToList();

                var ClientRevenueM = new List<ClientCoverageRevenueByMonth>();
                var ClientRevenue = new ClientCoverageRevenueByMonth();

                int cRId = 0;
                CM.ForEach(cm =>
                {
                    if (cm.CoverageId != cRId)
                    {
                        ClientRevenue = new ClientCoverageRevenueByMonth();
                        ClientRevenue.ClientName = cm.CleintName;
                        ClientRevenue.CoverageName = cm.CarrierName;
                        MapMonthRevenue(ClientRevenue,cm);
                        ClientRevenueM.Add(ClientRevenue);
                    }
                    else
                    {
                        MapMonthRevenue(ClientRevenue, cm);
                    }
                    cRId = cm.CoverageId;
                });
                c.ClientRevenueResultMonth = ClientRevenueM;
                c.Result = null;
            });


            commisions.Result = tempRs;
            return Json(commisions, JsonRequestBehavior.AllowGet);
        }


        public ActionResult MonthlyDetails()
        {
            var commisions = new SearchFiltrsResult();
            return View(commisions);
        }

        public void MapMonthlySummaryActual(MonthSummary summary, CommisionModel model)
        {

        }
        [HttpPost]
        public ActionResult MonthlyDetails(SearchFiltrs filters)
        {
            SearchFiltrsResult commisions = new SearchFiltrsResult()
            {
                DisplayTillMonth = filters.Month == 0 ? 12 : filters.Month,
                MonthSummaryResult = new List<MonthSummary>()
            };

            GetCommissions(filters, out commisions, out List<CommisionModel> data);
            commisions.MonthSummaryResult = new List<MonthSummary>();
            commisions.DisplayTillMonth = filters.Month == 0 ? 12 : filters.Month;
            data = data.Where(y => Convert.ToDateTime(y.AppliedDate).Year == filters.Year).ToList();
            data.ToList().ForEach(d =>
            {
                var isExist = commisions.MonthSummaryResult?.FirstOrDefault(cm => cm.PolicyNo == d.PolicyNumber);
                if (isExist == null)
                {
                    isExist = new MonthSummary()
                    {
                        PolicyNo = d.PolicyNumber,
                        CoverageName = d.CoverageName
                    };
                    commisions.MonthSummaryResult.Add(isExist);
                }
                   

                d.MapMonthlySummaryActual(isExist, _commonService);
                d.MapMonthlySummaryBudget(isExist, _commonService);
            });

            commisions.MonthSummaryResult = commisions.MonthSummaryResult.OrderBy(m => m.CoverageName).ToList();
            return Json(commisions, JsonRequestBehavior.AllowGet);
        }

        private void MapMonthRevenue(ClientCoverageRevenueByMonth rm, CommisionModel cm)
        {
            if (cm.StatementDate==null)
                return;

            switch (cm.StatementDate.Value.Month)
            {
                case 1:
                    rm.Jan = cm.CommisionValue??0;
                    break;
                case 2:
                    rm.Feb = cm.CommisionValue??0;
                    break;
                case 3:
                    rm.Mar = cm.CommisionValue??0;
                    break;
                case 4:
                    rm.Apr = cm.CommisionValue??0;
                    break;
                case 5:
                    rm.May = cm.CommisionValue??0;
                    break;
                case 6:
                    rm.Jun = cm.CommisionValue??0;
                    break;
                case 7:
                    rm.July = cm.CommisionValue??0;
                    break;
                case 8:
                    rm.Aug = cm.CommisionValue??0;
                    break;
                case 9:
                    rm.Sep = cm.CommisionValue??0;
                    break;
                case 10:
                    rm.Oct = cm.CommisionValue??0;
                    break;
                case 11:
                    rm.Nov = cm.CommisionValue??0;
                    break;
                case 12:
                    rm.Dec = cm.CommisionValue??0;
                    break;
            }
        } 
        public ActionResult ClientRevenueByMonths()
        {
            var commisions = new SearchFiltrsResult();
            return View(commisions);
        }
        [HttpPost]
        public ActionResult ClientRevenue(SearchFiltrs filters)
        {
            var commisions = new SearchFiltrsResult();

            GetCommissions(filters, out commisions, out List<CommisionModel> data);

            string cId = "";
            var tempRs = new List<GroupResult>();
            var tempR = new GroupResult();
            data.ToList().ForEach(d =>
            {
                if (cId != d.CleintName)
                {
                    tempR = new GroupResult();
                    tempR.Result = new List<CommisionModel>();
                    tempR.Result.Add(d);
                    tempRs.Add(tempR);
                }
                else
                {
                    tempR.Result.Add(d);
                }
                cId = d.CleintName;
            });

            tempRs.ForEach(c =>
            {
                var CM = c.Result.OrderBy(cm => cm.CoverageId).ToList();

                var ClientRevenueS = new List<ClientRevenue>();
                var ClientRevenue = new ClientRevenue();

                int cRId = 0;
                CM.ForEach(cm =>
                {
                    if (cm.CoverageId != cRId)
                    {
                        ClientRevenue = new ClientRevenue();
                        ClientRevenue.Result = new List<CommisionModel>();
                        ClientRevenue.Result.Add(cm);
                        ClientRevenueS.Add(ClientRevenue);
                    }
                    else
                    {
                        ClientRevenue.Result.Add(cm);
                    }
                    cRId = cm.CoverageId;
                });
                c.ClientRevenueResult = ClientRevenueS;
                c.Result = null;
            });
            commisions.Result = tempRs;

            return Json(commisions, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CoverageMonthlySummary()
        {
            return View();
        }
        private void GetCommissions(SearchFiltrs filters, out SearchFiltrsResult commisions, out List<CommisionModel> data)
        {
            commisions = new SearchFiltrsResult();
            var commissions = _commisionService.GetAllSavedCommissionsForCarrier(Convert.ToInt32(0)).OrderBy(c => c.ClientId).ToList();
            data = commissions;

            if (filters.StartDate == null)
                return;

            filters.DateTypes.ForEach(df =>
            {
                if (df == 1)
                {
                    commissions = commissions.Where(dd => dd.StatementDate >= Convert.ToDateTime(filters.StartDate) && dd.StatementDate <= Convert.ToDateTime(filters.EndDate)).ToList();
                }
                if (df == 2)
                {
                    commissions = commissions.Where(dd => dd.AppliedDate >= Convert.ToDateTime(filters.StartDate) && dd.AppliedDate <= Convert.ToDateTime(filters.EndDate)).ToList();
                }
            });

            if (filters.Partners != null && filters.Partners.Count > 0)
            {
                commissions = commissions.Where(dd => filters.Partners.Contains(dd.CarrierId)).ToList();
            }

            if (filters.Rtype != null && filters.Rtype.Count > 0)
            {
                commissions = commissions.Where(dd => filters.Rtype.Contains(dd.ReconsilationStatusId)).ToList();
            }

            if (filters.Divisions != null && filters.Divisions.Count > 0)
            {
                commissions = commissions.Where(dd => filters.Divisions.Contains(dd.DivisionId)).ToList();
            }


            if (filters.Git != null && filters.Git.Count == 1)
            {
                filters.Git.ForEach(g =>
                {
                    if (g == 1)
                    {
                        commissions = commissions.Where(dd => dd.Git == true).ToList();
                    }
                    if (g == 2)
                    {
                        commissions = commissions.Where(dd => dd.Git == false).ToList();
                    }
                });
            }
            data = commissions;
        }

        [HttpPost]
        public ActionResult GetPatrners()
        {
           var Partners = _carrierService.GetAllCarriers().ToList();
            return Json(Partners, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult GetDateTypes()
        {
            return Json(ListHelper.GetDateTypes(), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult GetDivisions()
        {

            return Json(ListHelper.GetDivisions(), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult GetRType()
        {

            return Json(ListHelper.RType(), JsonRequestBehavior.AllowGet);
        }
    }
}