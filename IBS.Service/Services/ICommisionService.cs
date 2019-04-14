using IBS.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBS.Service.Services
{
    public interface ICommisionService
    {
        List<CommisionModel> GetCarrierPoliciesById(int carrierId);
        bool SaveCommisions(List<CommisionModel> commissions);
    }
}
