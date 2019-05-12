using IBS.Core.Entities;
using IBS.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBS.Service.Services
{
    public interface ICommonService
    {
        IList<Coverage> GetAllCoverages();
        IList<Product> GetAllProducts();
        IList<Product> GetAllProductsByCoverageId(int coverageId);
        Coverage GetCoverageById(int coverageId);
        Product GetProductById(int productId);
        IList<ClientPolicie> GetAllClientPolicies();
        IList<ClientPolicie> GetAllClientPoliciesById(int clientId);
        bool AddClientPolocie(ClientPolicie clientPolicie);
        bool SoftRemoveClientPolicy(int policyId, int clientId);
        IList<ClientPolicyBudget> GetAllPolicyBudgets(int policyId);
        bool AddClientPolocyBudget(AddPolicyBudget budget);
        bool UpdateClientPolocyBudget(AddPolicyBudget budget);
        IList<ClientPolicyBudget> GetAllPolicyBudgetsForClientPolicyYear(int clientId,int policyId,int year);

        ClientPolicie GetClientPoliciesByPolicyId(int policyId);

        IList<CorporateProductsXProduct> GetAllCorporateXProducts();
    }
}
