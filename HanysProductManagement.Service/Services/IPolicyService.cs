using HanysProductManagement.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HanysProductManagement.Service.Services
{
    public interface IPolicyService
    {
        IList<PolicyModel> GetAllPolicies();
        PolicyModel GetById(int Id);
        bool AddPolicy(PolicyModel policy);
        bool ModifyPolicy(PolicyModel policy);
        bool DeletePolicy(int policyId);
    }
}
