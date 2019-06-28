using IBS.Core.Entities;
using IBS.Service.DataBaseContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBS.Service.Repositories
{
    public class ClientRepository : IClientRepository
    {
        private readonly IBSDbContext _hanysContext;
        public ClientRepository()
        {
            _hanysContext = SingletonForEF.Instance;
        }

        public bool Add(Client client)
        {
            _hanysContext.Clients.Add(client);
            _hanysContext.SaveChanges();
            return true;
        }

        public bool Delete(int id)
        {
            var client = new Client()
            {
                Id = id
            };

            _hanysContext.Clients.Attach(client);
            _hanysContext.Clients.Remove(client);
            _hanysContext.SaveChanges();
            return true;
        }

        public IQueryable<Client> GetAll()
        {
            return _hanysContext.Clients;
        }

        public Client GetById(int id)
        {
            var entity = GetAll().FirstOrDefault(c => c.Id == id);

            return entity;
        }

        public bool Update(Client client)
        {
            var data = _hanysContext.Clients.FirstOrDefault(c => c.Id == client.Id);
            if (data != null)
            {
                data.Name = client.Name;
                data.IsActive = client.IsActive;
                data.Division = client.Division;
                data.RevDate = client.RevDate;
                data.RevUser = client.RevUser;
                _hanysContext.SaveChanges();
            }
            return true;
        }
    }
}