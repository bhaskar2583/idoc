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
        public List<GroupResult> Result { get; set; }
    }

    public class GroupResult
    {
        public List<CommisionModel> Result { get; set; }
        public List<ClientRevenue> ClientRevenueResult { get; set; }
    }

    public class ClientRevenue
    {
        public List<CommisionModel> Result { get; set; }
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
    }
   
   
    public class ReportsController : Controller
    {
        private readonly ICarrierService _carrierService;
        private readonly ICommisionService _commisionService;
        public ReportsController(ICarrierService carrierService, ICommisionService commisionService)
        {
            _carrierService = carrierService;
            _commisionService = commisionService;

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
        public ActionResult ClientRevenue(SearchFiltrs filters)
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

        private void GetCommissions(SearchFiltrs filters, out SearchFiltrsResult commisions, out List<CommisionModel> data)
        {
            commisions = new SearchFiltrsResult();
            var commissions = _commisionService.GetAllSavedCommissionsForCarrier(Convert.ToInt32(0)).OrderBy(c => c.ClientId).ToList();
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