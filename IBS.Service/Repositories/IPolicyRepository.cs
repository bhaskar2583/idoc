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
        IQueryable<Policie> GetAll();
        Policie GetById(int id);
        bool Add(Policie policy);
        bool Update(Policie policy);
        bool Delete(int id);
    }
}
