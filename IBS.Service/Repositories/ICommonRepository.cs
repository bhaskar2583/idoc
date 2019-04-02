using IBS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBS.Service.Repositories
{
    public interface ICommonRepository
    {
        IList<Coverage> GetAllCoverages();
        IList<Product> GetAllProducts();
        Coverage GetCoverageById(int coverageId);
        Product GetProductById(int productId);
        IList<ClientPolicie> GetAllClientPolicies();
        IList<ClientPolicie> GetAllClientPoliciesById(int clientId);

        bool AddClientPolocie(ClientPolicie clientPolicie);
        bool SoftRemoveClientPolicy(int policyId, int clientId);

        IList<PolicieBudget> GetAllPolicyBudgets(int policyId);
    }
}
