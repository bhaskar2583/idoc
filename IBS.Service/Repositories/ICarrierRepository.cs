using IBS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBS.Service.Repositories
{
    public interface ICarrierRepository
    {
        IQueryable<Carrier> GetAll();
        Carrier GetById(int id);
        bool Add(Carrier carrier);
        bool Update(Carrier carrier);
        bool Delete(int id);
    }
}
