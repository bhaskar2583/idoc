using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using Web.Models;

namespace Web.Controllers
{
    public class ReportsController : Controller
    {
        private RolesServiceRepository db = new RolesServiceRepository();
        // GET: Reports
        public ActionResult Index()
        {
            ServiceRepository serviceObj = new ServiceRepository();
            ViewBag.Years = serviceObj.GetYears();

            OKTAServiceRepository okta = new OKTAServiceRepository();
            var Okta = okta.GetUserProfile(HttpContext.GetOwinContext().Authentication.User.Claims);
            var IsSuperAdmin = db.IsSuperAdmin(Okta.email);

            return View(new UserModel() { USR_IsAdmin = IsSuperAdmin });
        }


        public JsonResult GetChartData(int orgId, int fromYear, int toYear, int type)
        {
            try
            {
                StackedChartData obj = new StackedChartData();
                if (orgId > 0 && fromYear > 0 && toYear > 0)
                {
                    List<ReportVM> objData = new List<ReportVM>();
                    objData = TempData.Peek("Data") as List<ReportVM>;

                    ServiceRepository serviceObj = new ServiceRepository();

                    string APIData = serviceObj.GetServiceResponse(string.Format(@"report/{0}?from={1}&to={2}&type={3}", orgId, fromYear, toYear, type));

                    objData = JsonConvert.DeserializeObject<List<ReportVM>>(JsonConvert.DeserializeObject(APIData).ToString());
                    TempData["Data"] = objData;

                    obj.labels = GenerateLabels(fromYear, toYear, type == 3);// new string[12] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
                    obj.datasets = builLineChartDataPoints(objData, obj.labels);
                }
                return Json(obj, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public JsonResult GetGridData(int orgId, int fromYear, int toYear, int type)
        {
            try
            {
                List<ReportVM> objData = new List<ReportVM>();
                if (orgId > 0 && fromYear > 0 && toYear > 0)
                {
                    objData = TempData.Peek("Data") as List<ReportVM>;

                    ServiceRepository serviceObj = new ServiceRepository();

                    string APIData = serviceObj.GetServiceResponse(string.Format(@"report/{0}?from={1}&to={2}&type={3}", orgId, fromYear, toYear, type));

                    objData = JsonConvert.DeserializeObject<List<ReportVM>>(JsonConvert.DeserializeObject(APIData).ToString());
                    
                    GenerateEmptyLabels(objData, fromYear, toYear, type == 3);

                }
                return Json(objData.OrderBy(o => o.Year).ThenBy(t => t.Month), JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private List<MLookup> ColorsList { get; set; }

        private dataset[] builLineChartDataPoints(List<ReportVM> objData, string[] listOfMonths)
        {
            ColorsList = TempData.Peek("Colors") as List<MLookup>;
            if (ColorsList == null || ColorsList.Count == 0)
            {
                ColorsList = new List<MLookup>();
                ColorsList.Add(new MLookup() { ValueSet = "rgb(54, 162, 235)", Flag = true });
                ColorsList.Add(new MLookup() { ValueSet = "rgb(117, 27, 8)", Flag = true });
            }

            var Linecolors = ColorsList.Select(s => s.ValueSet).ToArray();

            List<dataset> objdatasetList = new List<dataset>();
            {
                dataset objMonthTotal = new dataset() { label = "NYSPFPRATE", backgroundColor = Linecolors[0], borderColor = Linecolors[0], fill = false, lineTension = 0 };
                decimal[] arrMonthTotal = new decimal[listOfMonths.Length];

                for (int j = 0; j < listOfMonths.Length; j++)
                {
                    string monthandYear = listOfMonths[j];
                    var filteredData = objData.Where(w => w.MONTHTEXT == monthandYear.Split(' ')[0] && w.Year == Convert.ToInt32(monthandYear.Split(' ')[1]));
                    arrMonthTotal[j] = (filteredData != null && filteredData.Count() > 0) ? Convert.ToDecimal(filteredData.Sum(S => Convert.ToDecimal(S.PFPRATE))) : 0;
                }

                objMonthTotal.data = arrMonthTotal;
                objdatasetList.Add(objMonthTotal);

                dataset objHospMonthTotal = new dataset() { label = "HOSPRATE", backgroundColor = Linecolors[1], borderColor = Linecolors[1], fill = false, lineTension = 0 };
                decimal[] arrHospMonthTotal = new decimal[listOfMonths.Length];

                for (int j = 0; j < listOfMonths.Length; j++)
                {
                    string monthandYear = listOfMonths[j];
                    var filteredData = objData.Where(w => w.MONTHTEXT == monthandYear.Split(' ')[0] && w.Year == Convert.ToInt32(monthandYear.Split(' ')[1]));
                    arrHospMonthTotal[j] = (filteredData != null && filteredData.Count() > 0) ? Convert.ToInt32(filteredData.Sum(S => Convert.ToDecimal(S.HOSPRATE))) : 0;
                }

                objHospMonthTotal.data = arrHospMonthTotal;
                objdatasetList.Add(objHospMonthTotal);
 
                return objdatasetList.ToArray();
            }
        }

        private string[] GenerateLabels(int fromyear, int toyear, bool isQuearterly)
        {
            int noOfYears = (toyear - fromyear + 1);
            string[] arrayMonths = new string[noOfYears * (isQuearterly ? 4 : 12)];

            for (int year = fromyear; year <= toyear; year++)
            {
                for (int month = 1; month <= (isQuearterly ? 4 : 12); month++)
                {
                    if (isQuearterly)
                    {
                        int currentIndex = ((year - fromyear) * 4) + month;
                        arrayMonths[currentIndex - 1] = "Q" + month + " " + Convert.ToString(year);
                    }
                    else
                    {
                        int currentIndex = ((year - fromyear) * 12) + month;
                        arrayMonths[currentIndex - 1] = yearMonths[month - 1] + " " + Convert.ToString(year);
                    }
                }
            }

            return arrayMonths;
        }

        private void GenerateEmptyLabels(List<ReportVM> objData, int fromyear, int toyear, bool isQuarterly)
        {
            int noOfYears = (toyear - fromyear + 1);

            for (int year = fromyear; year <= toyear; year++)
            {
                for (int month = 1; month <= (isQuarterly ? 4 : 12) ; month++)
                {
                    int currentIndex = ((year - fromyear) * (isQuarterly ? 4 : 12)) + month;
                    if (!objData.Exists(w => w.MONTHTEXT == (isQuarterly ? "Q" + month.ToString() : yearMonths[month - 1]) && w.Year == year))
                    {
                        objData.Add(new ReportVM() { DENOMINATOR = 0, HOSCOUNT = 0, HOSPRATE = 0, PFPRATE = 0, NUMERATOR = 0, Year = year, MONTHTEXT = (isQuarterly ? "Q" + month.ToString() : yearMonths[month - 1]), Month = month });
                    }
                }
            }
        }

        private string[] yearMonths = new string[12] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };

        public JsonResult Hospitals(string prefix)
        {

            OKTAServiceRepository okta = new OKTAServiceRepository();
            var Okta = okta.GetUserProfile(HttpContext.GetOwinContext().Authentication.User.Claims);
             var IsSuperAdmin = db.IsSuperAdmin(Okta.email);

            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = null;
            List<HospitalModel> hosiptals = null;

            if (IsSuperAdmin && prefix.Length > 2)
            {
                response = serviceObj.GetResponse(string.Format("hospital/0?prefix={0}", prefix, Okta.email));
                hosiptals = response.Content.ReadAsAsync<List<HospitalModel>>().Result;
                return Json(hosiptals, JsonRequestBehavior.AllowGet);
            }
            else if (!IsSuperAdmin)
            {
                response = serviceObj.GetResponse(string.Format("hospital/?email={0}", Okta.email));
                hosiptals = response.Content.ReadAsAsync<List<HospitalModel>>().Result;
                return Json(hosiptals, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(null, JsonRequestBehavior.AllowGet);
        }
    }
}
