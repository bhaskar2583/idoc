using IBS.Service.Helpers;
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
        private readonly List<SelectListCommon> DateTypes;
        private readonly List<SelectListCommon> RType;

        public ReportsController()
        {
            DateTypes = ListHelper.GetDateTypes();
            RType = ListHelper.RType();
        }
        // GET: Reports
        public ActionResult Index()
        {
            ViewBag.DateType = DateTypes;
            ViewBag.Companies = new List<SelectListCommon>();
            ViewBag.Agents = new List<SelectListCommon>();
            ViewBag.RType = RType;

            return View();
        }
    }
}