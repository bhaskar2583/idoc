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
    public class GroupResult
    {
        public List<CommisionModel> Result { get; set; }
    }
    public class SearchFiltrsResult
    {
        public List<GroupResult> Result { get; set; }
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
            var data = _commisionService.GetAllSavedCommissionsForCarrier(Convert.ToInt32(0)).OrderBy(c=>c.ClientId).ToList();

            filters.DateTypes.ForEach(df =>
            {
                if (df == 1)
                {
                    data = data.Where(dd => dd.StatementDate >= Convert.ToDateTime(filters.StartDate) && dd.StatementDate <= Convert.ToDateTime(filters.StartDate)).ToList();
                }
                if (df == 2)
                {
                    data = data.Where(dd => dd.AppliedDate >= Convert.ToDateTime(filters.StartDate) && dd.AppliedDate <= Convert.ToDateTime(filters.StartDate)).ToList();
                }

            });

            if (filters.Partners!=null && filters.Partners.Count > 0)
            {
                data = data.Where(dd => filters.Partners.Contains(dd.CarrierId)).ToList();
            }

            if (filters.Rtype!=null && filters.Rtype.Count > 0)
            {
                data = data.Where(dd => filters.Rtype.Contains(dd.ReconsilationStatusId)).ToList();
            }

            if (filters.Divisions != null && filters.Divisions.Count > 0)
            {
                data = data.Where(dd => filters.Divisions.Contains(dd.DivisionId)).ToList();
            }

            
            if (filters.Git != null && filters.Git.Count==1)
            {
                filters.Git.ForEach(g =>
                {
                    if (g == 1)
                    {
                        data = data.Where(dd => dd.Git==true).ToList();
                    }
                    if (g == 2)
                    {
                        data = data.Where(dd => dd.Git == false).ToList();
                    }
                });
            }

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