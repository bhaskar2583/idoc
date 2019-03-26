using IBS.Core.Entities;
using IBS.Core.Models;
using IBS.Service.Repositories;
using IBS.Service.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBS.Service.Services
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository _clientRepository;
        public ClientService(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }
        public bool AddClient(ClientModel client)
        {
            var entity = new Client()
            {
                Name = client.Name,
                IsActive = client.IsActive,
                Division = client.Division,
                AddUser = LoginUserDetails.GetWindowsLoginUserName(),
                AddDate = DateUtil.GetCurrentDate()
            };

            return _clientRepository.Add(entity);
        }

        public bool DeleteClient(int clientId)
        {
            return _clientRepository.Delete(clientId);
        }

        public ClientModel GetById(int Id)
        {
            var entity = _clientRepository.GetById(Id);

            if (entity != null)
            {
                return new ClientModel()
                {
                    Id = entity.Id,
                    IsActive = (bool)entity.IsActive,
                    Name = entity.Name,
                    Division = entity.Division
                };
            }
            return null;
        }

        public IList<ClientModel> GetAllClients()
        {
            var clients = new List<ClientModel>();

            var clientsData = _clientRepository.GetAll().ToList();

            if (clientsData == null)
            {
                return clients;
            }

            clientsData.ForEach(c =>
            {
                var client = new ClientModel()
                {
                    Id = c.Id,
                    IsActive = (bool)c.IsActive,
                    Name = c.Name,
                    Division = c.Division
                };

                client.SelectedDevision = client.Divisions.FirstOrDefault(d => d.Id == Convert.ToInt32(c.Division));

                clients.Add(client);
            });

            return clients;
        }

        public bool ModifyClient(ClientModel client)
        {
            var entity = new Client()
            {
                Id = client.Id,
                Name = client.Name,
                IsActive = client.IsActive,
                Division = client.Division,
                AddUser = client.AddUser,
                AddDate = client.AddDate,
                RevDate = DateUtil.GetCurrentDate(),
                RevUser = LoginUserDetails.GetWindowsLoginUserName()
            };

            return _clientRepository.Update(entity);
        }

    }
}
