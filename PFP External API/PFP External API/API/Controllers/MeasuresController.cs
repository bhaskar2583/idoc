namespace API.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.Description;
    using API.Models;
    using API.Models.DAL;
    using API.Models.EDM;

    public class MeasuresController : ApiController
    {
        private MeasuresDAL measuresDAL = new MeasuresDAL();

        // GET: api/Measures
        public List<HospitalVM> GetHospitalsData(string email = "")
        {
            return measuresDAL.GetHospitalsData(email);
        }

        // GET: api/Measures/1?&FromYear=0
        public IQueryable<Years> GetYears(int id, int FromYear = 0)
        {
            return measuresDAL.GetYears(FromYear);
        }

        // GET: api/Measures/1?&DisplayAll=false
        public IQueryable<MeasuresEventTypes> GetMeasuresEventTypes(int id, bool DisplayAll)
        {
            return measuresDAL.GetMeasuresEventTypes(DisplayAll);
        }

        // GET: api/Measures/1?&HosId=1&FromYear=2019
        public MeasuresData GetMeasuresData(int id, int HosId, int FromYear)
        {
            return measuresDAL.GetMeasuresData(id, HosId, FromYear);
        }

        // GET: api/Measures/1?&FromYear=2019&IsQuarterly=false
        public List<EventMeasureData> GetMeasuresData(int id, int FromYear, string TimePeriod)
        {
            return measuresDAL.GetMeasuresData(id, FromYear, TimePeriod);
        }

        // POST: api/Measures
        [ResponseType(typeof(HttpStatusCode))]
        public HttpStatusCode PostMeasuresData(List<MeasurementData> measurementDatas)
        {
            if (!ModelState.IsValid)
            {
                return HttpStatusCode.BadRequest;
            }

            try
            {
                measuresDAL.SaveMeasuresData(measurementDatas);
                return HttpStatusCode.OK;
            }
            catch(Exception)
            {
                return HttpStatusCode.BadRequest;
            }
        }
    }
}