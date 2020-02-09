namespace API.Controllers
{
    using API.Models;
    using API.Models.DAL;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;

    public class HospitalController : ApiController
    {
        private ManageHospitals hospitals = new ManageHospitals();
        // GET: api/Home
        public List<HospitalVM> GetUserMasters(int Id, string prefix)
        {
            return hospitals.GetHospitals(Id, prefix);
        }

        public List<HospitalVM> GetUserMasters(string email)
        {
            return hospitals.GetHospitals(email);
        }

        [HttpGet]
        public IEnumerable<Years> GetYears()
        {
            return new ManageHospitals().GetYears();
        }
    }
}
