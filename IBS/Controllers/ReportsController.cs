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
            return View();
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