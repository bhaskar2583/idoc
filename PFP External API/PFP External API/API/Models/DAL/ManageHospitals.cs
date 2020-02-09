using API.Models.EDM;
using System;
using System.Collections.Generic;
using System.Linq;

namespace API.Models.DAL
{
    public class ManageHospitals
    {
        private PFPEntities db = new PFPEntities();

        public List<HospitalVM> GetHospitals(int Id, string prefix)
        {
            var hospitalList = new List<HospitalVM>();
            var hospitals = Id == 1 ? db.Hospitals.Where(r => r.HOS_Active == true && r.HOS_HospitalName == prefix).ToList() :
                db.Hospitals.Where(r => r.HOS_Active == true && r.HOS_HospitalName.StartsWith(prefix)).ToList();

            hospitalList = hospitals.Select(x => new HospitalVM()
            {
                Id = x.HOS_Id,
                HospitalName = x.HOS_HospitalName,
                Parent_Id = x.HOS_Parent_Id
            }).ToList();
            return hospitalList;
        }

        public List<HospitalVM> GetHospitals(string email)
        {
            var user = db.Users.Where(r => r.USR_Active == true && r.USR_Email == email).FirstOrDefault();

            var hospitalList = new List<HospitalVM>();
            var hospitals = db.Hospitals.Where(r => r.HOS_Active == true && r.HOS_HospitalName == user.USR_OrganizationName).ToList();

            hospitalList = hospitals.Select(x => new HospitalVM()
            {
                Id = x.HOS_Id,
                HospitalName = x.HOS_HospitalName,
                Parent_Id = x.HOS_Parent_Id
            }).ToList();
            return hospitalList;
        }

        public IEnumerable<Years> GetYears()
        {
            List<Years> Years = new List<Years>();
            try
            {
                var emys = db.CalendarMasters.ToList();
                foreach (var emy in emys)
                {
                    Years.Add(new Years { Year = emy.CAL_Year });
                }
                var distinctEmys = Years.GroupBy(e => e.Year).Select(g => g.FirstOrDefault()).ToList();
                return distinctEmys.OrderBy(o => o.Year);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
