using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBS.Core.Entities;
using IBS.Service.Repositories;

namespace IBS.Service.Services
{
    public class CommonService : ICommonService
    {
        private readonly ICommonRepository _commonRepository;
        public CommonService(ICommonRepository commonRepository)
        {
            _commonRepository = commonRepository;
        }

        public bool AddClientPolocie(ClientPolicie clientPolicie)
        {
            return _commonRepository.AddClientPolocie(clientPolicie);
        }

        public bool SoftRemoveClientPolicy(int policyId, int clientId)
        {
            return _commonRepository.SoftRemoveClientPolicy(policyId, clientId);
        }

        public IList<ClientPolicie> GetAllClientPolicies()
        {
            return _commonRepository.GetAllClientPolicies();
        }

        public IList<ClientPolicie> GetAllClientPoliciesById(int clientId)
        {
            return _commonRepository.GetAllClientPoliciesById(clientId);
        }

        public IList<Coverage> GetAllCoverages()
        {
            return _commonRepository.GetAllCoverages();
        }

        public IList<Product> GetAllProducts()
        {
            return _commonRepository.GetAllProducts();
        }

        public Coverage GetCoverageById(int coverageId)
        {
            return _commonRepository.GetCoverageById(coverageId);
        }

        public Product GetProductById(int productId)
        {
            return _commonRepository.GetProductById(productId);
        }

        public IList<PolicieBudget> GetAllPolicyBudgets(int policyId)
        {
            return _commonRepository.GetAllPolicyBudgets(policyId);
        }

        public IList<Product> GetAllProductsByCoverageId(int coverageId)
        {
            return _commonRepository.GetAllProductsByCoverageId(coverageId);
        }
    }
}
