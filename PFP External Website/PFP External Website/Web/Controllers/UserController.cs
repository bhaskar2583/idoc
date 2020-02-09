using Newtonsoft.Json;
using PagedList;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Mvc;
using Web.Models;
using System.Configuration;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json.Linq;
using System;
using System.Web;

namespace Web.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private RolesServiceRepository db = new RolesServiceRepository();

        private readonly string PFPAPIuri = ConfigurationManager.AppSettings["PFP:PFPAPIuri"];
        private readonly string APIkey = ConfigurationManager.AppSettings["okta:APIkey"];
        private readonly string APIuri = ConfigurationManager.AppSettings["okta:APIuri"];
        private readonly string APPkey = ConfigurationManager.AppSettings["okta:APPkey"];
        private readonly string PFPGroupID = ConfigurationManager.AppSettings["PFP:GroupID"];
        


        // GET: User
        public ActionResult GetUsers(string sortOrder, string currentFilter, string searchString, int? page)
        {
            OKTAServiceRepository okta = new OKTAServiceRepository();
            var Okta = okta.GetUserProfile(HttpContext.GetOwinContext().Authentication.User.Claims);

            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.GetResponse("user?GetUserMasters");

            ViewBag.CurrentSort = sortOrder;
            ViewBag.FirstNameSortParm = (string.IsNullOrEmpty(sortOrder) || sortOrder == "FirstName") ? "FirstName_desc" : "FirstName";
            ViewBag.LastNameSortParm = sortOrder == "LastName" ? "LastName_desc" : "LastName";
            ViewBag.EmailSortParm = sortOrder == "Email" ? "Email_desc" : "Email";
            ViewBag.PhoneSortParm = sortOrder == "Phone" ? "Phone_desc" : "Phone";
            ViewBag.OrganizationNameSortParm = sortOrder == "OrganizationName" ? "OrganizationName_desc" : "OrganizationName";

            bool firstNameSearch = Request.Form.GetValues("FirstNameCheckbox") != null ? Request.Form.GetValues("FirstNameCheckbox")[0] == "on" : false;
            bool organizationNameSearch = Request.Form.GetValues("OrganizationNameCheckBox") != null ? Request.Form.GetValues("OrganizationNameCheckBox")[0] == "on" : false;
            bool lastNameSearch = Request.Form.GetValues("LastNameCheckbox") != null ? Request.Form.GetValues("LastNameCheckbox")[0] == "on" : false;
            bool emailSearch = Request.Form.GetValues("EmailCheckbox") != null ? Request.Form.GetValues("EmailCheckbox")[0] == "on" : false;

            if (searchString != null)
            {
                page = 1;
                if (firstNameSearch)
                    ViewBag.firstNameSearch = "Checked";

                if (organizationNameSearch)
                    ViewBag.organizationNameSearch = "Checked";

                if (lastNameSearch)
                    ViewBag.lastNameSearch = "Checked";

                if (emailSearch)
                    ViewBag.emailSearch = "Checked";
            }
            else
            {
                searchString = currentFilter;
                ViewBag.firstNameSearch = "Checked";
                ViewBag.organizationNameSearch = "Checked";
                ViewBag.lastNameSearch = "Checked";
                ViewBag.emailSearch = "Checked";
            }

            ViewBag.CurrentFilter = searchString;

            var users = response.Content.ReadAsAsync<List<UserModel>>().Result;

            // OKTA Call to get user ID from Email address *******************************************************************

            var client = new HttpClient();
            //assign the API URL call
            client.BaseAddress = new Uri(APIuri + "/v1/");
            //Set up the header for the API call
            client.DefaultRequestHeaders.Clear();
            //set up the API header by including the API key located at web.config
            client.DefaultRequestHeaders.Add("Authorization", "SSWS " + APIkey);

            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));


            string oktaID = "";
            string Geturl = "";

            foreach (UserModel user in users)
            {
                Geturl = "users?search=profile.email eq \"" + user.USR_Email + "\"";


                var response_okta = client.GetAsync(Geturl).Result;
                //initialized couple of variables
                var jsonResponse = "";

                List<Web.Models.OktaUser.Users> user_okta = new List<Web.Models.OktaUser.Users>();
                //user_okta = new Web.Models.OktaUser.Users();
                if (response_okta.IsSuccessStatusCode)
                {
                    //if the call was succesful get the results
                    jsonResponse = response_okta.Content.ReadAsStringAsync().Result;
                    //deserialized the JSON response into a list
                    user_okta = Newtonsoft.Json.JsonConvert.
                        DeserializeObject<List<Web.Models.OktaUser.Users>>(jsonResponse);

                    if (user_okta.Count >= 1)
                    user.USR_OKTAID = user_okta[0].id;
                }

            }



            if (!string.IsNullOrEmpty(searchString))
            {
                if (firstNameSearch && organizationNameSearch && lastNameSearch && emailSearch)
                {
                    users = users.Where(s => s.USR_FirstName.Replace(" ", string.Empty).ToLower().Contains(searchString.Replace(" ", string.Empty).ToLower()) || s.USR_OrganizationName.Replace(" ", string.Empty).ToLower().Contains(searchString.Replace(" ", string.Empty).ToLower()) || s.USR_LastName.Replace(" ", string.Empty).ToLower().Contains(searchString.Replace(" ", string.Empty).ToLower()) || s.USR_Email.Replace(" ", string.Empty).ToLower().Contains(searchString.Replace(" ", string.Empty).ToLower())).ToList();
                }
                else if (firstNameSearch && organizationNameSearch && lastNameSearch)
                {
                    users = users.Where(s => s.USR_FirstName.Replace(" ", string.Empty).ToLower().Contains(searchString.Replace(" ", string.Empty).ToLower()) || s.USR_OrganizationName.Replace(" ", string.Empty).ToLower().Contains(searchString.Replace(" ", string.Empty).ToLower()) || s.USR_LastName.Replace(" ", string.Empty).ToLower().Contains(searchString.Replace(" ", string.Empty).ToLower())).ToList();
                }
                else if (firstNameSearch && organizationNameSearch && emailSearch)
                {
                    users = users.Where(s => s.USR_FirstName.Replace(" ", string.Empty).ToLower().Contains(searchString.Replace(" ", string.Empty).ToLower()) || s.USR_OrganizationName.Replace(" ", string.Empty).ToLower().Contains(searchString.Replace(" ", string.Empty).ToLower()) || s.USR_Email.Replace(" ", string.Empty).ToLower().Contains(searchString.Replace(" ", string.Empty).ToLower())).ToList();
                }
                else if (firstNameSearch && emailSearch && lastNameSearch)
                {
                    users = users.Where(s => s.USR_FirstName.Replace(" ", string.Empty).ToLower().Contains(searchString.Replace(" ", string.Empty).ToLower()) || s.USR_Email.Replace(" ", string.Empty).ToLower().Contains(searchString.Replace(" ", string.Empty).ToLower()) || s.USR_LastName.Replace(" ", string.Empty).ToLower().Contains(searchString.Replace(" ", string.Empty).ToLower())).ToList();
                }
                else if (emailSearch && organizationNameSearch && lastNameSearch)
                {
                    users = users.Where(s => s.USR_Email.Replace(" ", string.Empty).ToLower().Contains(searchString.Replace(" ", string.Empty).ToLower()) || s.USR_OrganizationName.Replace(" ", string.Empty).ToLower().Contains(searchString.Replace(" ", string.Empty).ToLower()) || s.USR_LastName.Replace(" ", string.Empty).ToLower().Contains(searchString.Replace(" ", string.Empty).ToLower())).ToList();
                }
                else if (firstNameSearch && organizationNameSearch)
                {
                    users = users.Where(s => s.USR_FirstName.Replace(" ", string.Empty).ToLower().Contains(searchString.Replace(" ", string.Empty).ToLower()) || s.USR_OrganizationName.Replace(" ", string.Empty).ToLower().Contains(searchString.Replace(" ", string.Empty).ToLower())).ToList();
                }
                else if (firstNameSearch && emailSearch)
                {
                    users = users.Where(s => s.USR_FirstName.Replace(" ", string.Empty).ToLower().Contains(searchString.Replace(" ", string.Empty).ToLower()) ||
                    s.USR_Email.Replace(" ", string.Empty).ToLower().Contains(searchString.Replace(" ", string.Empty).ToLower())).ToList();
                }
                else if (lastNameSearch && emailSearch)
                {
                    users = users.Where(s => s.USR_LastName.Replace(" ", string.Empty).ToLower().Contains(searchString.Replace(" ", string.Empty).ToLower()) ||
                    s.USR_Email.Replace(" ", string.Empty).ToLower().Contains(searchString.Replace(" ", string.Empty).ToLower())).ToList();
                }
                else if (organizationNameSearch && emailSearch)
                {
                    users = users.Where(s => s.USR_OrganizationName.Replace(" ", string.Empty).ToLower().Contains(searchString.Replace(" ", string.Empty).ToLower()) ||
                    s.USR_Email.Replace(" ", string.Empty).ToLower().Contains(searchString.Replace(" ", string.Empty).ToLower())).ToList();
                }
                else if (lastNameSearch && organizationNameSearch)
                {
                    users = users.Where(s => s.USR_LastName.Replace(" ", string.Empty).ToLower().Contains(searchString.Replace(" ", string.Empty).ToLower()) || s.USR_OrganizationName.Replace(" ", string.Empty).ToLower().Contains(searchString.Replace(" ", string.Empty).ToLower())).ToList();
                }
                else if (firstNameSearch && lastNameSearch)
                {
                    users = users.Where(s => s.USR_FirstName.Replace(" ", string.Empty).ToLower().Contains(searchString.Replace(" ", string.Empty).ToLower()) || s.USR_LastName.Replace(" ", string.Empty).ToLower().Contains(searchString.Replace(" ", string.Empty).ToLower())).ToList();
                }
                else if (firstNameSearch)
                {
                    users = users.Where(s => s.USR_FirstName.Replace(" ", string.Empty).ToLower().Contains(searchString.Replace(" ", string.Empty).ToLower())).ToList();
                }
                else if (lastNameSearch)
                {
                    users = users.Where(s => s.USR_LastName.Replace(" ", string.Empty).ToLower().Contains(searchString.Replace(" ", string.Empty).ToLower())).ToList();
                }
                else if (emailSearch)
                {
                    users = users.Where(s => s.USR_Email.Replace(" ", string.Empty).ToLower().Contains(searchString.Replace(" ", string.Empty).ToLower())).ToList();
                }
                else if (organizationNameSearch)
                {
                    users = users.Where(s => s.USR_OrganizationName.Replace(" ", string.Empty).ToLower().Contains(searchString.Replace(" ", string.Empty).ToLower())).ToList();
                }
            }


            switch (sortOrder)
            {
                case "FirstName":
                    users = users.OrderBy(m => m.USR_FirstName).ToList();
                    break;
                case "FirstName_desc":
                    users = users.OrderByDescending(m => m.USR_FirstName).ToList();
                    break;
                case "LastName":
                    users = users.OrderBy(m => m.USR_LastName).ToList();
                    break;
                case "LastName_desc":
                    users = users.OrderByDescending(m => m.USR_LastName).ToList();
                    break;
                case "Email":
                    users = users.OrderBy(m => m.USR_Email).ToList();
                    break;
                case "Email_desc":
                    users = users.OrderByDescending(m => m.USR_Email).ToList();
                    break;
                case "Phone":
                    users = users.OrderBy(m => m.USR_Email).ToList();
                    break;
                case "Phone_desc":
                    users = users.OrderByDescending(m => m.USR_Email).ToList();
                    break;
                case "OrganizationName":
                    users = users.OrderBy(m => m.USR_Email).ToList();
                    break;
                case "OrganizationName_desc":
                    users = users.OrderByDescending(m => m.USR_Email).ToList();
                    break;
                case "Default":
                    users = users.OrderBy(m => m.USR_FirstName).ToList();
                    break;
            }
            if (!db.IsSuperAdmin(Okta.email))
                users = new List<UserModel>();
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(users.ToPagedList(pageNumber, pageSize));
        }

        public List<Roles> GetRoles()
        {
            List<Roles> roles = new List<Roles>();

            ServiceRepository api = new ServiceRepository();
            var APIData = api.GetServiceResponse("roles");
            //var APIData = ("{ \"Roles\" : " + api.GetServiceResponse("roles") + " }");
            List<Roles> objUserRoles = JsonConvert.DeserializeObject<List<Roles>>(JsonConvert.DeserializeObject(APIData).ToString());
            return objUserRoles;

        }

        [HttpGet]
        public ActionResult Create()
        {
            OKTAServiceRepository okta = new OKTAServiceRepository();
            var Okta = okta.GetUserProfile(HttpContext.GetOwinContext().Authentication.User.Claims);
            var IsSuperAdmin = db.IsSuperAdmin(Okta.email);
            if (!IsSuperAdmin)
                ViewBag.RoleExistMessage = "Please contact HANYS Admin to create";

            List<Roles> objRoles = GetRoles();
            objRoles.Add(new Roles() { Id = 0, Name = "Please Select a Role" });
            return View(new UserModel() { Roles = objRoles.OrderBy(o => o.Id).ToList() });
        }

        [HttpPost]
        public ActionResult Create(UserModel user)
        {
            OKTAServiceRepository okta = new OKTAServiceRepository();
            var Okta = okta.GetUserProfile(HttpContext.GetOwinContext().Authentication.User.Claims);
            var IsSuperAdmin = db.IsSuperAdmin(Okta.email);
            if (!IsSuperAdmin)
                ViewBag.RoleExistMessage = "Please contact HANYS Admin to create";

            // UserServiceRepository serviceObj = new UserServiceRepository();
            // user.USR_Active = true;
            //HttpResponseMessage response = serviceObj.PostResponse("user/PostUserMaster", user);
            // response.EnsureSuccessStatusCode();
            int flag = 0;

           // return RedirectToAction("GetUsers");
            if (ModelState.IsValid && !string.IsNullOrEmpty(user.USR_Email) && !IsUserExists(user) && ValidateHospital(user.USR_OrganizationName) && IsSuperAdmin)
            {
                ServiceRepository serviceObj = new ServiceRepository();
                user.USR_Active = true;

                HttpResponseMessage response = serviceObj.PostResponse("user/PostUserMaster", user);
                response.EnsureSuccessStatusCode();

                var result = response.Content.ReadAsStringAsync().Result;
                user = JsonConvert.DeserializeObject<UserModel>(result);



                if (response.StatusCode == System.Net.HttpStatusCode.Created)
                {

                    var client = new HttpClient();
                    //assign the API URL call
                    client.BaseAddress = new Uri(APIuri + "/v1/");
                    //Set up the header for the API call
                    client.DefaultRequestHeaders.Clear();
                    //set up the API header by including the API key located at web.config
                    client.DefaultRequestHeaders.Add("Authorization", "SSWS " + APIkey);

                    client.DefaultRequestHeaders.Accept.Add(
                        new MediaTypeWithQualityHeaderValue("application/json"));

                    string Geturl = "";

                    Geturl = "users?search=profile.email eq \"" + user.USR_Email + "\"";


                    var response_okta = client.GetAsync(Geturl).Result;
                    //initialized couple of variables
                    var jsonResponse = "";

                    List<Web.Models.OktaUser.Users> user_okta = new List<Web.Models.OktaUser.Users>();
                    //user_okta = new Web.Models.OktaUser.Users();
                    if (response_okta.IsSuccessStatusCode)
                    {
                        //if the call was succesful get the results
                        jsonResponse = response_okta.Content.ReadAsStringAsync().Result;

                        if (jsonResponse.Length > 2)
                        {
                            //deserialized the JSON response into a list
                            user_okta = Newtonsoft.Json.JsonConvert.
                                DeserializeObject<List<Web.Models.OktaUser.Users>>(jsonResponse);


                            if (user_okta[0].status != "ACTIVE")
                            {

                                string PostuserActivation;
                                PostuserActivation = "users/" + user_okta[0].id + "/lifecycle/activate?sendEmail=true";

                                string jsonActive = "";

                                var httpContent = new StringContent(jsonActive, Encoding.Default, "application/json");

                                response = client.PostAsync(PostuserActivation, httpContent).Result;
                                //initialized couple of variables
                                jsonResponse = "";
                                response.Content.ToString();

                                if (response.IsSuccessStatusCode)
                                {
                                    //if the call was succesful get the results
                                    jsonResponse = response.Content.ReadAsStringAsync().Result;
                                    //Message on successful user creation
                                    //var messageModel = new MessageModel();


                                }
                            }

                            // assign user to pfp application

                            string Postusergrp;
                            Postusergrp = "groups/" + PFPGroupID + "/users/" + user_okta[0].id;



                            string jsonuser = "";

                            StringContent httpContentuser = new StringContent(jsonuser, Encoding.Default, "application/json");

                            response = client.PutAsync(Postusergrp, httpContentuser).Result;
                            //initialized couple of variables
                            jsonResponse = "";
                            response.Content.ToString();

                            if (response.IsSuccessStatusCode)
                            {
                                //if the call was succesful get the results
                                jsonResponse = response.Content.ReadAsStringAsync().Result;
                                //Message on successful user creation
                                //var messageModel = new MessageModel();


                            }






                        }
                        else
                        {
                            // New user Creation in OKTA



                            CreateOktaUser OktaUser = new CreateOktaUser();





                            OktaUser.email = user.USR_Email;
                            OktaUser.firstName = user.USR_FirstName;
                            OktaUser.lastName = user.USR_LastName;
                            OktaUser.login = user.USR_Email;


                            string json = Newtonsoft.Json.JsonConvert.SerializeObject(OktaUser);

                            json = json.Replace("{", "{\"profile\": {");
                            json = json.Replace("}", "}}");
                            var httpContent = new StringContent(json, Encoding.Default, "application/json");

                            string Postuser;
                            Postuser = "users?activate=true";
                            //Call the Okta apps API to get a list of all available apps
                            response = client.PostAsync(Postuser, httpContent).Result;
                            //initialized couple of variables
                            jsonResponse = "";
                            response.Content.ToString();

                            if (response.IsSuccessStatusCode)
                            {
                                //if the call was succesful get the results
                                jsonResponse = response.Content.ReadAsStringAsync().Result;
                                //Message on successful user creation
                                //var messageModel = new MessageModel();


                            }

                            var obj = JObject.Parse(jsonResponse);
                            var ID = (string)obj["id"];

                            // assign user to pfp application

                            string Postusergrp;
                            Postusergrp = "groups/" + PFPGroupID + "/users/" + ID;



                            string jsonuser = "";

                            StringContent httpContentuser = new StringContent(jsonuser, Encoding.Default, "application/json");

                            response = client.PutAsync(Postusergrp, httpContentuser).Result;
                            //initialized couple of variables
                            jsonResponse = "";
                            response.Content.ToString();

                            if (response.IsSuccessStatusCode)
                            {
                                //if the call was succesful get the results
                                jsonResponse = response.Content.ReadAsStringAsync().Result;
                                //Message on successful user creation
                                //var messageModel = new MessageModel();


                            }


                        }
                    }

                }

                return RedirectToAction("Index", "Userroles", new { Id = user.USR_Id });
            }
            else
                return View(user);
        }

        [HttpGet]
        public ActionResult EditUser(int id)
        {
            OKTAServiceRepository okta = new OKTAServiceRepository();
            var Okta = okta.GetUserProfile(HttpContext.GetOwinContext().Authentication.User.Claims);
            var IsSuperAdmin = db.IsSuperAdmin(Okta.email);
            if (!IsSuperAdmin)
                ViewBag.RoleExistMessage = "Please contact HANYS Admin to edit";

            var user = new UserModel();

            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.GetResponse("user/GetUserById?id=" + id);
            user = response.Content.ReadAsAsync<UserModel>().Result;

            var client = new HttpClient();
            //assign the API URL call
            client.BaseAddress = new Uri(APIuri + "/v1/");
            //Set up the header for the API call
            client.DefaultRequestHeaders.Clear();
            //set up the API header by including the API key located at web.config
            client.DefaultRequestHeaders.Add("Authorization", "SSWS " + APIkey);

            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));



            string Geturl = "";

            Geturl = "users?search=profile.email eq \"" + user.USR_Email + "\"";


            var response_okta = client.GetAsync(Geturl).Result;
            //initialized couple of variables
            var jsonResponse = "";

            List<Web.Models.OktaUser.Users> user_okta = new List<Web.Models.OktaUser.Users>();
            //user_okta = new Web.Models.OktaUser.Users();
            if (response_okta.IsSuccessStatusCode)
            {
                //if the call was succesful get the results
                jsonResponse = response_okta.Content.ReadAsStringAsync().Result;
                //deserialized the JSON response into a list
                user_okta = Newtonsoft.Json.JsonConvert.
                    DeserializeObject<List<Web.Models.OktaUser.Users>>(jsonResponse);

                user.USR_OKTAID = user_okta[0].id;
            }

                return View(user);
        }

 
        public ActionResult EditUser(UserModel user)
        {
            OKTAServiceRepository okta = new OKTAServiceRepository();
            var Okta = okta.GetUserProfile(HttpContext.GetOwinContext().Authentication.User.Claims);
            var IsSuperAdmin = db.IsSuperAdmin(Okta.email);
            if (!IsSuperAdmin)
                ViewBag.RoleExistMessage = "Please contact HANYS Admin to edit";

            if (ModelState.IsValid && !string.IsNullOrEmpty(user.USR_Email) && !IsUserExists(user) && ValidateHospital(user.USR_OrganizationName) && IsSuperAdmin)
            {
                ServiceRepository serviceObj = new ServiceRepository();
                user.USR_Active = true;
                HttpResponseMessage response = serviceObj.PutResponse("user/PutUserMaster", user);



                var client = new HttpClient();
                //assign the API URL call
                client.BaseAddress = new Uri(APIuri + "/v1/");
                //Set up the header for the API call
                client.DefaultRequestHeaders.Clear();
                //set up the API header by including the API key located at web.config
                client.DefaultRequestHeaders.Add("Authorization", "SSWS " + APIkey);

                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));





                CreateOktaUser OktaUser = new CreateOktaUser();

                OktaUser.email = user.USR_Email;
                OktaUser.firstName = user.USR_FirstName;
                OktaUser.lastName = user.USR_LastName;
                OktaUser.login = user.USR_Email;
                OktaUser.secondEmail = null;
                OktaUser.middleName = null;
                OktaUser.mobilePhone = null;


                string json = Newtonsoft.Json.JsonConvert.SerializeObject(OktaUser);

                json = json.Replace("{", "{\"profile\": {");
                json = json.Replace("}", "}}");
                var httpContent = new StringContent(json, Encoding.Default, "application/json");

                string Postuser;
                Postuser = "users/" + user.USR_OKTAID;
                //Call the Okta apps API to get a list of all available apps
                response = client.PostAsync(Postuser, httpContent).Result;

                //initialized couple of variables
                var jsonResponse = "";
                response.Content.ToString();

                if (response.IsSuccessStatusCode)
                {
                    //if the call was succesful get the results
                    jsonResponse = response.Content.ReadAsStringAsync().Result;
                    //Message on successful user creation
                    //var messageModel = new MessageModel();


                }
           

                return RedirectToAction("Index", "Userroles", new { Id = user.USR_Id });
            }
            else
                return View(user);
        }

        public ActionResult Delete(int id, string email)
        {
            OKTAServiceRepository okta = new OKTAServiceRepository();
            var Okta = okta.GetUserProfile(HttpContext.GetOwinContext().Authentication.User.Claims);
            var IsSuperAdmin = db.IsSuperAdmin(Okta.email);
            if (!IsSuperAdmin)
                ViewBag.RoleExistMessage = "Please contact HANYS Admin to delete";

            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.DeleteResponse("user/DeleteUserMaster?id=" + id);
     



            var client = new HttpClient();
            //assign the API URL call
            client.BaseAddress = new Uri(APIuri + "/v1/");
            //Set up the header for the API call
            client.DefaultRequestHeaders.Clear();
            //set up the API header by including the API key located at web.config
            client.DefaultRequestHeaders.Add("Authorization", "SSWS " + APIkey);

            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));


            string oktaID = "";
            string Geturl = "";

            Geturl = "users?search=profile.email eq \"" + email + "\"";


            var response_okta = client.GetAsync(Geturl).Result;
            //initialized couple of variables
            var jsonResponse = "";

            List<Web.Models.OktaUser.Users> user_okta = new List<Web.Models.OktaUser.Users>();
            //user_okta = new Web.Models.OktaUser.Users();
            if (response_okta.IsSuccessStatusCode)
            {
                //if the call was succesful get the results
                jsonResponse = response_okta.Content.ReadAsStringAsync().Result;
                //deserialized the JSON response into a list
                user_okta = Newtonsoft.Json.JsonConvert.
                    DeserializeObject<List<Web.Models.OktaUser.Users>>(jsonResponse);

                oktaID = user_okta[0].id;
            }


           string Postuser ="";

            Postuser = "groups/" + PFPGroupID  + "/users/" + oktaID;
            //Call the Okta apps API to get a list of all available apps

            response = client.DeleteAsync(Postuser).Result;
            //initialized couple of variables
            jsonResponse = "";

            response.Content.ToString();
            if (response.IsSuccessStatusCode)
            {
                jsonResponse = response.Content.ReadAsStringAsync().Result;
            }


            return RedirectToAction("GetUsers");


        }



        public ActionResult ReActivateUser(string id)
        {
            OKTAServiceRepository okta = new OKTAServiceRepository();
            var Okta = okta.GetUserProfile(HttpContext.GetOwinContext().Authentication.User.Claims);
            var IsSuperAdmin = db.IsSuperAdmin(Okta.email);
            if (!IsSuperAdmin)
                ViewBag.RoleExistMessage = "Please contact HANYS Admin to reactive user";

            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}

            var client = new HttpClient();
            //assign the API URL call
            client.BaseAddress = new Uri(APIuri + "/v1/");
            //Set up the header for the API call
            client.DefaultRequestHeaders.Clear();
            //set up the API header by including the API key located at web.config
            client.DefaultRequestHeaders.Add("Authorization", "SSWS " + APIkey);

            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            string json = "";

            var httpContent = new StringContent(json, Encoding.Default, "application/json");

            string Postuser;
            Postuser = "users/" + id + "/lifecycle/reset_password?sendEmail=true";
            //Call the Okta apps API to get a list of all available apps
            var response = client.PostAsync(Postuser, httpContent).Result;
            //initialized couple of variables
            var jsonResponse = "";
            response.Content.ToString();
            if (response.IsSuccessStatusCode)
            {
                //if the call was succesful get the results
                jsonResponse = response.Content.ReadAsStringAsync().Result;
            }


            return RedirectToAction("GetUsers");
        }

        private bool IsUserExists(UserModel user)
        {
            ServiceRepository api = new ServiceRepository();
            var APIData = api.GetServiceResponse(string.Format("user/{0}?email={1}", user.USR_Id, user.USR_Email));
            bool result = JsonConvert.DeserializeObject<bool>(JsonConvert.DeserializeObject(APIData).ToString());
            ViewBag.EmailExistMessage = result == true ? "Email aleady exist" : string.Empty;
            return result;
        }

        [HttpPost]
        public JsonResult Hospitals(string prefix, int OnlyUserHospitals)
          {
            if (prefix.Length > 2)
            {
                ServiceRepository serviceObj = new ServiceRepository();
                HttpResponseMessage response = serviceObj.GetResponse(string.Format("hospital/0?prefix={0}", prefix));

                var hosiptals = response.Content.ReadAsAsync<List<HospitalModel>>().Result;
                return Json(hosiptals, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(null, JsonRequestBehavior.AllowGet);
        }


        public bool ValidateHospital(string prefix)
        {
            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.GetResponse(string.Format("hospital/1?prefix={0}", prefix));
            var hosiptals = response.Content.ReadAsAsync<List<HospitalModel>>().Result;

            bool hospitalExist = hosiptals.Count() > 0;
            ViewBag.HospitalValidation = hospitalExist ? "" : "Invalid OrganizationName";
            return hospitalExist;
        }
    }
}