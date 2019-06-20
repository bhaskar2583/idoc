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
        private readonly ICommonService _commonService;
        private readonly IPolicyService _policyService;
        public ClientService(IClientRepository clientRepository, ICommonService commonService,
            IPolicyService policyService)
        {
            _clientRepository = clientRepository;
            _commonService = commonService;
            _policyService = policyService;
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
                if(!string.IsNullOrEmpty(c.Division))
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

        public ClientModel GetAllClientPolocies(int clientId)
        {
            var clientResult = new ClientModel();
            var client = GetById(clientId);

            if (client == null)
                return clientResult;
            clientResult.Name = client.Name;
            clientResult.Id = client.Id;

            var clietPolocies = _commonService.GetAllClientPoliciesById(clientId).Where(cp => cp.IsActive == true);

            clietPolocies.ToList().ForEach(cli => {
                var policie = _policyService.GetById(cli.PolicieId);
                policie.ClientPolicyId = cli.Id;
                clientResult.Polocies.Add(policie);
            });
            return clientResult;
        }

        public bool AddClientPolocie(int clientId, int polocieId)
        {
            var clientPolocieModel = new ClientPolicie()
            {
                ClientId = clientId,
                PolicieId = polocieId,
                IsActive = true,
                AddUser = LoginUserDetails.GetWindowsLoginUserName(),
                AddDate = DateUtil.GetCurrentDate()
            };

           return _commonService.AddClientPolocie(clientPolocieModel);
        }

        public bool SoftRemoveClientPolicy(int policyId, int clientId)
        {
            return _commonService.SoftRemoveClientPolicy(policyId, clientId);
        }
    }
}
