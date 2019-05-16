using IBS.Core.Entities;
using IBS.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBS.Service.Repositories
{
    public interface ICommisionRepository
    {
        bool Add(Commision commission);
        Commision GetByClientPolicyId(int clientPolicyId);
        bool Update(CommisionModel commission);
        List<Commision> GetSavedCommisionsForCarrier(int carrierId);
        List<Commision> GetByAllClientPolicyId(int clientPolicyId);
    }
}
