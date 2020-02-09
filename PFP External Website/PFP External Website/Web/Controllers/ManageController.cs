namespace Web.Controllers
{
    using System.Web.Mvc;

    [Authorize]
    public class ManageController : BaseController
    {
        // GET: Manage
        public ActionResult Index()
        {
            IsSuperAdminUser();
            return View();
        }
    }
}
