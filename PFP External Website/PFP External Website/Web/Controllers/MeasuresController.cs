namespace Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Web;
    using System.Web.Mvc;
    using Web.Models;

    [Authorize]
    public class MeasuresController : BaseController
    {
        private RolesServiceRepository roleDB = new RolesServiceRepository();
        private MeasuresServiceRepository db = new MeasuresServiceRepository();

        // GET: Measures
        public ActionResult Index()
        {
            return View(db.GetMeasuresEventTypes());
        }

        // GET: Measures/Update/?&HosId=0&FromYear=2020&TP=
        [HttpGet]
        public ActionResult Update(int HosId = 0, int FromYear = 0, string TP = "")
        {
            if (HosId < 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.FromYear = db.GetYears(FromYear);
            ViewBag.TP = db.GetTimePeriods(TP);

            var eventMeasureDatas = new List<EventMeasureData>();
            if (HosId > 0 || FromYear > 0)
                eventMeasureDatas = db.GetMeasuresData(HosId, FromYear, TP);

            return View(eventMeasureDatas);
        }

        [HttpPost]
        public ActionResult Update(List<EventMeasureData> etms, int HosId = 0, int FromYear = 0, string TP = "")
        {
            if (ModelState.IsValid)
            {
                if (db.PostMeasuresData(etms))
                    return RedirectToAction("Index");
            }

            ViewBag.FromYear = db.GetYears(FromYear);
            ViewBag.TP = db.GetTimePeriods(TP);
            return View(etms);
        }

        // GET: Measures/Edit/1
        public ActionResult Edit(int? id, int HosId = 0, int FromYear = 0)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            OKTAServiceRepository okta = new OKTAServiceRepository();
            var Okta = okta.GetUserProfile(HttpContext.GetOwinContext().Authentication.User.Claims);
            if (!roleDB.IsSuperAdmin(Okta.email))
                ViewBag.Hospitals = db.GetHospitals(HosId, Okta.email);
            else
                ViewBag.Hospitals = db.GetHospitals(HosId, string.Empty);
            ViewBag.FromYear = db.GetYears(FromYear);

            MeasuresData measuresData = new MeasuresData();
            try
            {
                measuresData = db.GetMeasuresData((int)id, HosId, FromYear);
            }
            catch (Exception ex)
            {
                measuresData = new MeasuresData() { Measures = new List<Measure>() };
                Console.WriteLine(ex);
            }
            return View(measuresData);
        }
    }
}
