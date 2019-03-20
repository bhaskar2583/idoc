using IBS.Core.Entities;
using IBS.Core.Models;
using IBS.Service.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBS.Service.Services
{
    public class HospitalService : IHospitalService
    {
        private readonly IHospitalRepository _hospitalRepository;
        public HospitalService(IHospitalRepository hospitalRepository)
        {
            _hospitalRepository = hospitalRepository;
        }
        public bool AddHospital(HospitalModel hospital)
        {
            var entity = new Hospital()
            {
                Name = hospital.Name,
                Active = hospital.Active
            };

            return _hospitalRepository.Add(entity);
        }

        public bool DeleteHospital(int hospitalId)
        {
            return _hospitalRepository.Delete(hospitalId);
        }

        public HospitalModel GetById(int Id)
        {
            var entity = _hospitalRepository.GetById(Id);

            if (entity != null)
            {
                return new HospitalModel()
                {
                    Id = entity.Id,
                    Active = (bool)entity.Active,
                    Name = entity.Name
                };
            }

            return null;

        }

        public IList<HospitalModel> GetAllHospitals()
        {
            var hospitals = new List<HospitalModel>();

            var hospitalsData = _hospitalRepository.GetAll().ToList();

            if (hospitalsData == null)
            {
                return hospitals;
            }

            hospitalsData.ForEach(c =>
            {
                var hospital = new HospitalModel()
                {
                    Id = c.Id,
                    Active = (bool)c.Active,
                    Name = c.Name
                };

                hospitals.Add(hospital);
            });

            return hospitals;
        }

        public bool ModifyHospital(HospitalModel hospital)
        {
            var entity = new Hospital()
            {
                Id = hospital.Id,
                Name = hospital.Name,
                Active = hospital.Active
            };

            return _hospitalRepository.Update(entity);
        }
    }
}
