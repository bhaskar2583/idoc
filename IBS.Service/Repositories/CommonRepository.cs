using IBS.Core.Entities;
using IBS.Service.DataBaseContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBS.Service.Repositories
{
    public class CommonRepository : ICommonRepository
    {
        private readonly IBSDbContext _hanysContext;
        public CommonRepository()
        {
            _hanysContext = new IBSDbContext();
        }

        public bool AddClientPolocie(ClientPolicie clientPolicie)
        {
           _hanysContext.ClientPolicies.Add(clientPolicie);
            _hanysContext.SaveChanges();
            return true;
        }

        public IList<ClientPolicie> GetAllClientPolicies()
        {
            return _hanysContext.ClientPolicies.ToList();
        }

        public IList<ClientPolicie> GetAllClientPoliciesById(int clientId)
        {
            return _hanysContext.ClientPolicies.Where(cov => cov.ClientId == clientId).ToList();
        }

        public IList<Coverage> GetAllCoverages()
        {
            return _hanysContext.Coverages.ToList();
         }

        public IList<Product> GetAllProducts()
        {
            return _hanysContext.Products.ToList();
        }

        public Coverage GetCoverageById(int coverageId)
        {
            return _hanysContext.Coverages.FirstOrDefault(cov => cov.Id == coverageId);
        }

        public Product GetProductById(int productId)
        {
            return _hanysContext.Products.FirstOrDefault(cov => cov.Id == productId);
        }
    }
}
