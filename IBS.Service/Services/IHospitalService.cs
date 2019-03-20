using IBS.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBS.Service.Services
{
    public interface IHospitalService
    {
        IList<HospitalModel> GetAllHospitals();
        HospitalModel GetById(int Id);
        bool AddHospital(HospitalModel hospital);
        bool ModifyHospital(HospitalModel hospital);
        bool DeleteHospital(int hospitalId);
    }
}
