using IBS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBS.Service.Repositories
{
    public interface IClientRepository
    {
        IQueryable<Client> GetAll();
        Client GetById(int id);
        bool Add(Client client);
        bool Update(Client client);
        bool Delete(int id);
    }
}
