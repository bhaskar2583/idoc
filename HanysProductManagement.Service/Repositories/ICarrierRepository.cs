using HanysProductManagement.Service.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HanysProductManagement.Service.Repositories
{
    public interface ICarrierRepository
    {
        IQueryable<Carriers> GetAll();
        Carriers GetById(int id);
        bool Add(Carriers carrier);
        bool Update(Carriers carrier);
        bool Delete(int id);
    }
}
