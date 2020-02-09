namespace Web.Controllers
{
    using PagedList;
    using System.Linq;
    using System.Net;
    using System.Web;
    using System.Web.Mvc;
    using Web.Models;
    using System.Data;
    using System.Collections.Generic;

    [Authorize]
    public class RolesController : BaseController
    {
        private RolesServiceRepository db = new RolesServiceRepository();
        public ActionResult Index(string SortOrder, string CurrentFilter, string SearchString, int? Page, bool SearchRoleName = true, bool SearchDescription = true)
        {
            OKTAServiceRepository okta = new OKTAServiceRepository();
            var Okta = okta.GetUserProfile(HttpContext.GetOwinContext().Authentication.User.Claims);

            ViewBag.CurrentSort = SortOrder;
            ViewBag.NameSortParm = (string.IsNullOrEmpty(SortOrder) || SortOrder == "Name") ? "Name_desc" : "Name";
            ViewBag.DescriptionSortParm = SortOrder == "Description" ? "Description_desc" : "Description";
            ViewBag.UserCountSortParm = SortOrder == "UserCount" ? "UserCount_desc" : "UserCount";
            ViewBag.ActiveTextSortParm = SortOrder == "ActiveText" ? "ActiveText_desc" : "ActiveText";
            if (SearchRoleName == false && SearchDescription == false)
            {
                SearchRoleName = true;
                SearchDescription = true;
            }
            if (SearchString != null)
                Page = 1;
            else
                SearchString = CurrentFilter;
            ViewBag.CurrentFilter = SearchString;
            ViewBag.RoleNameFilter = SearchRoleName;
            ViewBag.DescriptionFilter = SearchDescription;

            var roles = db.GetRoles();
            if (!string.IsNullOrEmpty(SearchString))
            {
                SearchString = SearchString.Replace(" ", string.Empty).ToLower();
                if (SearchRoleName == true && SearchDescription == true)
                    roles = roles.Where(s => s.Name.Replace(" ", string.Empty).ToLower().Contains(SearchString) || s.Description.Replace(" ", string.Empty).ToLower().Contains(SearchString)).ToList();
                else if (SearchRoleName == true)
                    roles = roles.Where(s => s.Name.Replace(" ", string.Empty).ToLower().Contains(SearchString)).ToList();
                else if (SearchDescription == true)
                    roles = roles.Where(s => s.Description.Replace(" ", string.Empty).ToLower().Contains(SearchString)).ToList();
            }

            switch (SortOrder)
            {
                case "Name":
                    roles = roles.OrderBy(s => s.Name).ToList();
                    break;
                case "Name_desc":
                    roles = roles.OrderByDescending(s => s.Name).ToList();
                    break;
                case "Description":
                    roles = roles.OrderBy(s => s.Description).ToList();
                    break;
                case "Description_desc":
                    roles = roles.OrderByDescending(s => s.Description).ToList();
                    break;
                case "UserCount":
                    roles = roles.OrderBy(s => s.UserCount).ToList();
                    break;
                case "UserCount_desc":
                    roles = roles.OrderByDescending(s => s.UserCount).ToList();
                    break;
                case "ActiveText":
                    roles = roles.OrderBy(s => s.Active).ToList();
                    break;
                case "ActiveText_desc":
                    roles = roles.OrderByDescending(s => s.Active).ToList();
                    break;
                default:
                    roles = roles.OrderBy(s => s.Name).ToList();
                    break;
            }
            
            if (!db.IsSuperAdmin(Okta.email))
                roles = new List<Roles>();
            int PageNumber = (Page ?? 1);
            int PageSize = 10;
            return View(roles.ToPagedList(PageNumber, PageSize));
        }

        public ActionResult Details(int id)
        {
            if (id <= 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Roles role = db.GetRole(id);
            if (role == null)
            {
                return HttpNotFound();
            }
            return View(role);
        }

        public ActionResult Create()
        {
            //Roles role = new Roles();
            OKTAServiceRepository okta = new OKTAServiceRepository();
            var Okta = okta.GetUserProfile(HttpContext.GetOwinContext().Authentication.User.Claims);
            var IsSuperAdmin = db.IsSuperAdmin(Okta.email);
            if (!IsSuperAdmin)
                ViewBag.RoleExistMessage = "Please contact HANYS Admin to create";
            return View();
        }

        [HttpPost]
        public ActionResult Create([Bind(Include = "Id,Name,Description")] Roles role)
        {
            OKTAServiceRepository okta = new OKTAServiceRepository();
            var Okta = okta.GetUserProfile(HttpContext.GetOwinContext().Authentication.User.Claims);
            var IsSuperAdmin = db.IsSuperAdmin(Okta.email);
            if (Okta.userName.Length > 0 && ModelState.IsValid && !IsRoleExists(role.Name) && IsSuperAdmin)
            {
                role.Active = true;
                role.CreatedBy = Okta.userName;
                role.UpdatedBy = Okta.userName;
                db.PostRole(role);
                return RedirectToAction("Index");
            }

            if(!IsSuperAdmin)
                ViewBag.RoleExistMessage = "Please contact HANYS Admin to create";
            return View(role);
        }

        public ActionResult Edit(int id)
        {
            Roles role = new Roles();
            OKTAServiceRepository okta = new OKTAServiceRepository();
            var Okta = okta.GetUserProfile(HttpContext.GetOwinContext().Authentication.User.Claims);
            var IsSuperAdmin = db.IsSuperAdmin(Okta.email);
            if (Okta.userName.Length > 0 && IsSuperAdmin)
            {
                if (id <= 0)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                role = db.GetRole(id);
                if (role == null)
                {
                    return HttpNotFound();
                }
            }
            if (!IsSuperAdmin)
                ViewBag.RoleExistMessage = "Please contact HANYS Admin to edit";
            role.Current = role.Name;
            return View(role);
        }

        [HttpPost]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,Active,Current")] Roles role)
        {
            OKTAServiceRepository okta = new OKTAServiceRepository();
            var Okta = okta.GetUserProfile(HttpContext.GetOwinContext().Authentication.User.Claims);
            var IsSuperAdmin = db.IsSuperAdmin(Okta.email);
            if (Okta.userName.Length > 0 && ModelState.IsValid && (role.Name == role.Current || (role.Name != role.Current && !IsRoleExists(role.Name))) && IsSuperAdmin)
            {
                role.CreatedBy = Okta.userName;
                role.UpdatedBy = Okta.userName;

                db.PutRole(role.Id, role);
                return RedirectToAction("Index");
            }
            if (!IsSuperAdmin)
                ViewBag.RoleExistMessage = "Please contact HANYS Admin to edit";
            return View(role);
        }

        public ActionResult Delete(int id)
        {
            Roles role = new Roles();
            OKTAServiceRepository okta = new OKTAServiceRepository();
            var Okta = okta.GetUserProfile(HttpContext.GetOwinContext().Authentication.User.Claims);
            var IsSuperAdmin = db.IsSuperAdmin(Okta.email);
            if (Okta.userName.Length > 0 && IsSuperAdmin)
            {
                if (id <= 0)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                role = db.GetRole(id);
                if (role == null)
                {
                    return HttpNotFound();
                }
            }
            if (!IsSuperAdmin)
                ViewBag.RoleExistMessage = "Please contact HANYS Admin to delete";
            return View(role);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            OKTAServiceRepository okta = new OKTAServiceRepository();
            var Okta = okta.GetUserProfile(HttpContext.GetOwinContext().Authentication.User.Claims);
            var IsSuperAdmin = db.IsSuperAdmin(Okta.email);
            if (Okta.userName.Length > 0 && IsSuperAdmin)
            {
                db.DeleteRole(id);
            }
            if (!IsSuperAdmin)
                ViewBag.RoleExistMessage = "Please contact HANYS Admin to delete";
            return RedirectToAction("Index");
        }

        private bool IsRoleExists(string roleName)
        {
            bool flag = db.IsRoleExists(roleName.ToLower().Replace(" ", string.Empty));
            ViewBag.RoleExistMessage = flag == true ? "Role name aleady exist" : string.Empty;
            return flag;
        }
    }
}
