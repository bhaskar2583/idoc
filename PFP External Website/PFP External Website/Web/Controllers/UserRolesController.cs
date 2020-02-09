using Newtonsoft.Json;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using Web.Models;

namespace Web.Controllers
{
    [Authorize]
    public class UserRolesController : Controller
    {
        private RolesServiceRepository db = new RolesServiceRepository();

        // GET: UserRoles
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page, string Id)
        {
            OKTAServiceRepository okta = new OKTAServiceRepository();
            var Okta = okta.GetUserProfile(HttpContext.GetOwinContext().Authentication.User.Claims);

            ServiceRepository api = new ServiceRepository();
            var APIData = api.GetServiceResponse("userroles");

            ViewBag.Id = Id;

            ViewBag.CurrentSort = sortOrder;
            ViewBag.UserNameSortParm = (string.IsNullOrEmpty(sortOrder) || sortOrder == "UserName") ? "UserName_desc" : "UserName";
            ViewBag.RoleSortParm = sortOrder == "Role" ? "Role_desc" : "Role";
            ViewBag.OrganizationNameSortParm = sortOrder == "OrganizationName" ? "OrganizationName_desc" : "OrganizationName";

            if (searchString != null)
                page = 1;
            else
                searchString = currentFilter;
            ViewBag.CurrentFilter = searchString;

            var userroles = JsonConvert.DeserializeObject<List<UserRole>>(JsonConvert.DeserializeObject(APIData).ToString());

            if (Convert.ToInt32(Id) > 0)
                userroles = userroles.Where(w => w.UserID == (Convert.ToInt32(Id))).ToList();
            if (!string.IsNullOrEmpty(searchString))
                userroles = userroles.Where(s => s.UserName.Replace(" ", string.Empty).ToLower().Contains(searchString.Replace(" ", string.Empty).ToLower()) || s.RoleName.Replace(" ", string.Empty).ToLower().Contains(searchString.Replace(" ", string.Empty).ToLower()) || s.OrganizationName.Replace(" ", string.Empty).ToLower().Contains(searchString.Replace(" ", string.Empty).ToLower())).ToList();

            switch (sortOrder)
            {
                case "UserName":
                    userroles = userroles.OrderBy(m => m.UserName).ToList();
                    break;
                case "UserName_desc":
                    userroles = userroles.OrderByDescending(m => m.UserName).ToList();
                    break;
                case "Role":
                    userroles = userroles.OrderBy(m => m.RoleName).ToList();
                    break;
                case "Role_desc":
                    userroles = userroles.OrderByDescending(m => m.RoleName).ToList();
                    break;
                case "OrganizationName":
                    userroles = userroles.OrderBy(m => m.OrganizationName).ToList();
                    break;
                case "OrganizationName_desc":
                    userroles = userroles.OrderByDescending(m => m.OrganizationName).ToList();
                    break;
                case "Default":
                    userroles = userroles.OrderBy(m => m.UserName).ToList();
                    break;
            }


            int pageSize = 10;
            int pageNumber = (page ?? 1);

            UserRoleModel obj = new UserRoleModel()
            {
                UserRoles = userroles.ToPagedList(pageNumber, pageSize),
                UserRole = GetUserrole(Convert.ToInt32(Id))
            };

            if (!db.IsSuperAdmin(Okta.email))
                obj = new UserRoleModel();

            return View(obj);
        }

        public UserRole GetUserrole(int userId)
        {
            List<Roles> objRoles = GetRoles();
            objRoles.Add(new Roles() { Id = 0, Name = "Please Select a Role" });

            List<UserModel> objUsers = GetUsers();
            objUsers.Add(new UserModel() { USR_Id = 0, USR_UserName = "Please Select a User" });
            return new UserRole() { UserID = userId, Roles = objRoles.OrderBy(o => o.Id).ToList(), Users = objUsers.Where(w => w.USR_Id == userId).OrderBy(o => o.USR_Id).ToList() };
        }

        public List<Roles> GetRoles()
        {
            RolesServiceRepository db = new RolesServiceRepository();
            var roles = db.GetRoles();
            return roles.Where(w => w.Active == true).ToList();
        }

        public List<UserModel> GetUsers()
        {
            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.GetResponse("user?GetUserMasters");
            var users = response.Content.ReadAsAsync<List<UserModel>>().Result;
            return users;
        }

        [HttpPost]
        public ActionResult Create(UserRoleModel objuserRole)
        {
            OKTAServiceRepository okta = new OKTAServiceRepository();
            var Okta = okta.GetUserProfile(HttpContext.GetOwinContext().Authentication.User.Claims);
            var IsSuperAdmin = db.IsSuperAdmin(Okta.email);
            if (!IsSuperAdmin)
                ViewBag.RoleExistMessage = "Please contact HANYS Admin to create";

            UserRole userRole = objuserRole.UserRole;
            ServiceRepository api = new ServiceRepository();

            if (userRole.Id > 0)
            {

                ServiceRepository serviceObj = new ServiceRepository();
                HttpResponseMessage response = serviceObj.PutResponse("userroles/PutUserRole", userRole);
                response.EnsureSuccessStatusCode();
                return new JsonResult { Data = new { status = true, message = "Role updated succesfully" } };
            }
            else
            {
                var APIData = api.GetServiceResponse("userroles?id=" + userRole.UserID + "&roleId=" + userRole.RoleID);
                bool isUserRoleExist = JsonConvert.DeserializeObject<bool>(JsonConvert.DeserializeObject(APIData).ToString());
                if (isUserRoleExist)
                {
                    ViewBag.RoleExistMessage = "Role  already assigned to this user";
                    //return View();
                }
                else
                {
                    ServiceRepository serviceObj = new ServiceRepository();
                    HttpResponseMessage response = serviceObj.PostResponse("userroles/PostUserRole", userRole);
                    response.EnsureSuccessStatusCode();
                }
               
            }

            return RedirectToAction("Index", new { Id = userRole.UserID });
        }

        public ActionResult Delete(int id, int? usrId)
        {
            OKTAServiceRepository okta = new OKTAServiceRepository();
            var Okta = okta.GetUserProfile(HttpContext.GetOwinContext().Authentication.User.Claims);
            var IsSuperAdmin = db.IsSuperAdmin(Okta.email);
            if (!IsSuperAdmin)
                ViewBag.RoleExistMessage = "Please contact HANYS Admin to delete";

            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.DeleteResponse("userroles/DeleteUserRole?id=" + id);
            if (usrId == 0)
                return RedirectToAction("Index");
            else
                return RedirectToAction("Index", new { Id = usrId });
        }
    }
}