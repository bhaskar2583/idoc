using IBS.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBS.Service.Services
{
    public interface IClientService
    {
        IList<ClientModel> GetAllClients();
        ClientModel GetById(int Id);
        bool AddClient(ClientModel client);
        bool ModifyClient(ClientModel client);
        bool DeleteClient(int clientId);

        ClientModel GetAllClientPolocies(int clientId);
        bool AddClientPolocie(int clientId, int polocieId);
        bool SoftRemoveClientPolicy(int policyId,int clientId);
    }
}
