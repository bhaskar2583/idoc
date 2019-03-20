using IBS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBS.Service.Repositories
{
    public interface IPolicyRepository
    {
        IQueryable<Policy> GetAll();
        Policy GetById(int id);
        bool Add(Policy policy);
        bool Update(Policy policy);
        bool Delete(int id);
    }
}
