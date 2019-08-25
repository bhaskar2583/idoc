using IBS.Core.Entities;
using IBS.Core.Models;
using IBS.Service.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBS.Service.Repositories
{
    public interface ICommisionRepository
    {
        List<Commision> GetSavedCommisions();
        bool Add(Commision commission);
        Commision GetByClientPolicyId(int clientPolicyId);
        bool Update(CommisionModel commission);
        List<Commision> GetSavedCommisionsForCarrier(int carrierId);
        List<Commision> GetByAllClientPolicyId(int clientPolicyId);
        bool UpdateCommissionReconsilation(CommisionModel commission);
        List<InvalidCommission> GetExceptionCommissionsForCarrier(int carrierId);
        InvalidCommission GetExceptionCommissionsById(int Id);
        List<int> GetExceptionCommissionsCariers();
        List<SelectListCommon> GetExceptionCarrierStatementDates(int carrierId);
        bool UpdateExceptionCommisions(InvalidCommission commission);
        bool UpdateExceptionCommisionsClient(InvalidCommission commission);
        List<InvalidCommission> GetExceptionCommissionsByPolicyNo(string policyNo);
    }
}