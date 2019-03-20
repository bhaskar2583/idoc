using IBS.Core.Entities;
using IBS.Service.DataBaseContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBS.Service.Repositories
{
    public class HospitalRepository : IHospitalRepository
    {
        private readonly IBSDbContext _hanysContext;
        public HospitalRepository()
        {
            _hanysContext = new IBSDbContext();
        }

        public bool Add(Hospital hospital)
        {
            _hanysContext.Hospitals.Add(hospital);
            _hanysContext.SaveChanges();
            return true;
        }

        public bool Delete(int id)
        {
            var hospital = new Hospital()
            {
                Id = id
            };

            _hanysContext.Hospitals.Attach(hospital);
            _hanysContext.Hospitals.Remove(hospital);
            _hanysContext.SaveChanges();
            return true;
        }

        public IQueryable<Hospital> GetAll()
        {
            return _hanysContext.Hospitals;
        }

        public Hospital GetById(int id)
        {
            var entity = GetAll().FirstOrDefault(c => c.Id == id);

            return entity;
        }

        public bool Update(Hospital hospital)
        {
            var data = _hanysContext.Hospitals.FirstOrDefault(c => c.Id == hospital.Id);
            if (data != null)
            {
                data.Name = hospital.Name;
                data.Active = hospital.Active;
                _hanysContext.SaveChanges();
            }
            return true;
        }
    }
}
