namespace Web.Controllers
{
    using System;
    using System.Web;
    using System.Web.Mvc;
    using Web.Models;

    public class BaseController : Controller
    {
        private RolesServiceRepository db = new RolesServiceRepository();
        private MeasuresServiceRepository msr_db = new MeasuresServiceRepository();

        public ActionResult IsSuperAdminUser()
        {
            try
            {
                OKTAServiceRepository oktaSR = new OKTAServiceRepository();
                var Okta = oktaSR.GetUserProfile(HttpContext.GetOwinContext().Authentication.User.Claims);
                ViewBag.IsSuperAdminUser = db.IsSuperAdmin(Okta.email) ? true : false;
                return PartialView("admin");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ActionResult HospitalSelection(int HosId = 0)
        {
            try
            {
                OKTAServiceRepository okta = new OKTAServiceRepository();
                var Okta = okta.GetUserProfile(HttpContext.GetOwinContext().Authentication.User.Claims);
                if (!db.IsSuperAdmin(Okta.email))
                    ViewBag.Hospitals = msr_db.GetHospitals(HosId, Okta.email);
                else
                    ViewBag.Hospitals = msr_db.GetHospitals(HosId, string.Empty);

                return PartialView("HospitalSelection");
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}