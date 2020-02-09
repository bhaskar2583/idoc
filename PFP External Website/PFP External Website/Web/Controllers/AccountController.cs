using Microsoft.Owin.Security.Cookies;
using Okta.AspNet;
using System.Web;
using System.Web.Mvc;

namespace Web.Controllers
{

    [Authorize]
    public class AccountController : BaseController
    {

        [HttpPost]
        public ActionResult Logout()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                HttpContext.GetOwinContext().Authentication.SignOut(
                    CookieAuthenticationDefaults.AuthenticationType,OktaDefaults.MvcAuthenticationType);
            }

            return RedirectToAction("Index", "Home");
        }

        public ActionResult PostLogout()
        {
            return RedirectToAction("Index", "Home");
        }
    }
}