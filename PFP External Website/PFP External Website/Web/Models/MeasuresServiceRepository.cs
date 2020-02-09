namespace Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Mvc;

    public class MeasuresServiceRepository
    {
        private HttpClient Client { get; set; }
        public MeasuresServiceRepository()
        {
            Client = new HttpClient
            {
                BaseAddress = new Uri(ConfigurationManager.AppSettings["PFP:PFPAPIuri"].ToString())
            };
        }

        internal IEnumerable<SelectListItem> GetHospitals(int hosId = 0, string email = "")
        {
            HttpResponseMessage response = Client.GetAsync("Measures/?&email=" + email).Result;
            List<HospitalModel> hospitalModels = response.Content.ReadAsAsync<List<HospitalModel>>().Result;

            List<SelectListItem> listHospitals = new List<SelectListItem>();
            foreach (var hospital in hospitalModels)
                if (hospital.Id == hosId)
                    listHospitals.Add(new SelectListItem { Value = hospital.Id.ToString(), Text = hospital.HospitalName.ToString(), Selected = true });
                else
                    listHospitals.Add(new SelectListItem { Value = hospital.Id.ToString(), Text = hospital.HospitalName.ToString() });
            return listHospitals;
        }

        internal IEnumerable<SelectListItem> GetYears(int Year = 0)
        {
            HttpResponseMessage response = Client.GetAsync("Measures/1?&FromYear=0").Result;
            List<Years> years = response.Content.ReadAsAsync<List<Years>>().Result;
            years = years.OrderByDescending(y => y.Year).ToList();
            Year = Year == 0 ? DateTime.Now.Year : Year;
            List<SelectListItem> listYears = new List<SelectListItem>();
            foreach (var yr in years)
            {
                if (yr.Year == Year)
                    listYears.Add(new SelectListItem { Value = yr.Year.ToString(), Text = yr.Year.ToString(), Selected = true });
                else
                    listYears.Add(new SelectListItem { Value = yr.Year.ToString(), Text = yr.Year.ToString() });
            }
            return listYears;
        }

        internal IEnumerable<SelectListItem> GetTimePeriods(string TP)
        {
            List<Periods> allPeriods = new List<Periods>();
            allPeriods = TimePeriods.GetTimePeriods();

            List<SelectListItem> listPeriods = new List<SelectListItem>();
            foreach (var period in allPeriods)
            {
                if (period.PeriodId == TP)
                    listPeriods.Add(new SelectListItem { Value = period.PeriodId, Text = period.PeriodText, Selected = true });
                else
                    listPeriods.Add(new SelectListItem { Value = period.PeriodId, Text = period.PeriodText });
            }
            return listPeriods;
        }

        internal IEnumerable<MeasuresEventTypes> GetMeasuresEventTypes(bool DisplayAll = false)
        {
            HttpResponseMessage response = Client.GetAsync("Measures/1?&DisplayAll=" + DisplayAll).Result;
            return response.Content.ReadAsAsync<List<MeasuresEventTypes>>().Result;
        }

        internal MeasuresData GetMeasuresData(int id, int HosId, int FromYear)
        {
            HttpResponseMessage response = Client.GetAsync("Measures/" + id + "?&HosId=" + HosId + "&FromYear=" + FromYear).Result;
            MeasuresData md = response.Content.ReadAsAsync<MeasuresData>().Result;
            foreach (var m in md.Measures)
            {
                List<SelectListItem> listAPM = new List<SelectListItem>();
                foreach (var a in m.AnalysisPeriod)
                    if (m.AnalysisPeriodId != null && a.AnalysisPeriodId == m.AnalysisPeriodId)
                        listAPM.Add(new SelectListItem { Text = a.AnalysisPeriodName, Value = a.AnalysisPeriodId.ToString(), Selected = true });
                    else
                        listAPM.Add(new SelectListItem { Text = a.AnalysisPeriodName, Value = a.AnalysisPeriodId.ToString() });
                m.AnalysisPeriodSelectListItems = new SelectList(listAPM, "Value", "Text");

            }
            return md;
        }

        internal List<EventMeasureData> GetMeasuresData(int id, int FromYear, string TimePeriod)
        {
            HttpResponseMessage response = Client.GetAsync("Measures/" + id + "?&FromYear=" + FromYear + "&TimePeriod=" + TimePeriod).Result;
            List<EventMeasureData> eventMeasureDatas = response.Content.ReadAsAsync<List<EventMeasureData>>().Result;
            return eventMeasureDatas;
        }

        internal bool PostMeasuresData(List<EventMeasureData> etms)
        {
            List<MeasurementData> measurementDatas = new List<MeasurementData>();

            for (var i = 0; i < etms.Count(); i++)
                for (var k = 0; k < etms[i].MeasurementDatas.Count(); k++)
                {
                    measurementDatas.Add(new MeasurementData()
                    {
                        HosId = etms[i].MeasurementDatas[k].HosId,
                        CalId = etms[i].MeasurementDatas[k].CalId,
                        EmmId = etms[i].MeasurementDatas[k].EmmId,
                        Numerator = etms[i].MeasurementDatas[k].Numerator,
                        Denominator = etms[i].MeasurementDatas[k].Denominator,
                        Multiplier = etms[i].MeasurementDatas[k].Multiplier,
                        Measurement = etms[i].MeasurementDatas[k].Denominator == 0 ? 0.0m : ((etms[i].MeasurementDatas[k].Numerator / etms[i].MeasurementDatas[k].Denominator) * etms[i].MeasurementDatas[k].Multiplier),
                        UpdatedBy = "HANYSNT\\Ksuriyak"
                    });
                }

            HttpResponseMessage response = Client.PostAsJsonAsync("Measures", measurementDatas).Result;
            return response.Content.ReadAsAsync<HttpStatusCode>().Result == HttpStatusCode.OK ? true : false;
        }
    }
}