using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Web.Mvc;

namespace Web.Models
{
    public class ServiceRepository
    {
        private readonly string PFPAPIuri = ConfigurationManager.AppSettings["PFP:PFPAPIuri"];

        public HttpClient Client { get; set; }
        public ServiceRepository()
        {
            Client = new HttpClient();
            Client.BaseAddress = new Uri(ConfigurationManager.AppSettings["PFP:PFPAPIuri"].ToString());
        }
        public HttpResponseMessage GetResponse(string url)
        {
            return Client.GetAsync(url).Result;
        }
        public HttpResponseMessage PutResponse(string url, object model)
        {
            return Client.PutAsJsonAsync(url, model).Result;
        }
        public HttpResponseMessage PostResponse(string url, object model)
        {
            return Client.PostAsJsonAsync(url, model).Result;
        }
        public HttpResponseMessage DeleteResponse(string url)
        {
            return Client.DeleteAsync(url).Result;
        }

        public string GetServiceResponse(string url)
        {
            var responseString = string.Empty;
            try
            {
                HttpClient client = new HttpClient();
                ServicePointManager.ServerCertificateValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;
                Uri uri = new Uri(PFPAPIuri + url);

                HttpWebRequest request = HttpWebRequest.Create(uri.ToString()) as HttpWebRequest;
                request.Timeout = Convert.ToInt32(new TimeSpan(0, 30, 0).TotalMilliseconds);
                request.Method = "GET";

                WebResponse webResponse = request.GetResponse();
                responseString = new StreamReader(webResponse.GetResponseStream()).ReadToEnd();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return responseString;
        }

        public IEnumerable<SelectListItem> GetYears(int Year = 0)
        {

            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.GetResponse("Hospital/GetYears");

            List<Years> years = new List<Years>();
            years = JsonConvert.DeserializeObject<List<Years>>(GetServiceResponse("Hospital/GetYears"));

            List<SelectListItem> listYears = new List<SelectListItem>();
            foreach (var yr in years)
            {
                if (Year == yr.Year)
                {
                    listYears.Add(new SelectListItem { Value = yr.Year.ToString(), Text = yr.Year.ToString(), Selected = true });
                }
                else
                {
                    listYears.Add(new SelectListItem { Value = yr.Year.ToString(), Text = yr.Year.ToString() });
                }
            }
            return listYears;
        }

    }
}