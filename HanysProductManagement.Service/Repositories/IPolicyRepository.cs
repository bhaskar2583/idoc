using HanysProductManagement.Service.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HanysProductManagement.Service.Repositories
{
    public interface IPolicyRepository
    {
        IQueryable<Policy> GetAll();
        Policy GetById(int id);
        bool Add(Policy carrier);
        bool Update(Policy carrier);
        bool Delete(int id);
    }
}
