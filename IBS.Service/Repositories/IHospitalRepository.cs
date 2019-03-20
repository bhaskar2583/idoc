using IBS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBS.Service.Repositories
{
    public interface IHospitalRepository
    {
        IQueryable<Hospital> GetAll();
        Hospital GetById(int id);
        bool Add(Hospital carrier);
        bool Update(Hospital carrier);
        bool Delete(int id);
    }
}
