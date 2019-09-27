using IBS.Core.Entities;
using IBS.Service.DataBaseContext;
using IBS.Service.Utils;
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

        public bool AddCoverages(Coverage coverage)
        {
            _hanysContext.Coverages.Add(coverage);
            _hanysContext.SaveChanges();
            return true;
        }

        public bool UpdateCoverages(Coverage coverage)
        {
            var data = _hanysContext.Coverages.FirstOrDefault(c => c.Id == coverage.Id);
            if (data != null)
            {
                data.Name = coverage.Name;
                _hanysContext.SaveChanges();
            }
            return true;
        }
        public bool AddProduct(Product product)
        {
            _hanysContext.Products.Add(product);
            _hanysContext.SaveChanges();
            return true;
        }

        public bool UpdateProduct(Product product)
        {
            var data = _hanysContext.Products.FirstOrDefault(c => c.Id == product.Id);
            if (data != null)
            {
                data.Name = product.Name;
                data.CoverageId = product.CoverageId;
                _hanysContext.SaveChanges();
            }
            return true;
        }

        public bool AddClientPolocie(ClientPolicie clientPolicie)
        {
            _hanysContext.ClientPolicies.Add(clientPolicie);
            _hanysContext.SaveChanges();
            return true;
        }

        public bool SoftRemoveClientPolicy(int policyId, int clientId)
        {

            var data = _hanysContext.ClientPolicies.FirstOrDefault(c => c.PolicieId == policyId && c.ClientId == clientId);
            if (data != null)
            {
                data.IsActive = false;
                data.RevDate = DateUtil.GetCurrentDate();
                data.RevUser = LoginUserDetails.GetWindowsLoginUserName();
                _hanysContext.SaveChanges();
            }
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

        public IList<ClientPolicie> GetAllClientPoliciesByIndexId(int id)
        {
            return _hanysContext.ClientPolicies.Where(cov => cov.Id == id).ToList();
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

        public IList<ClientPolicyBudget> GetAllPolicyBudgets(int policyId)
        {
            return _hanysContext.ClientPolicieBudgets.Where(p => p.PolicyId == policyId).ToList();
        }

        public IList<Product> GetAllProductsByCoverageId(int coverageId)
        {
            return _hanysContext.Products.Where(p => p.CoverageId == coverageId).ToList();
        }

        public bool AddClientPolocyBudget(ClientPolicyBudget budget)
        {
            var data = _hanysContext.ClientPolicieBudgets.FirstOrDefault(c => c.PolicyId == budget.PolicyId
               && c.BudgetYear == budget.BudgetYear
               && c.BudgetMonth == budget.BudgetMonth);

            if (data != null)
            {
                data.IsActive = budget.IsActive;
                data.BudgetValue = budget.BudgetValue;
                data.RevDate = budget.RevDate;
                data.RevUser = budget.RevUser;
                _hanysContext.SaveChanges();
                return true;
            }

            _hanysContext.ClientPolicieBudgets.Add(budget);
            _hanysContext.SaveChanges();
            return true;
        }

        public bool UpdateClientPolocyBudget(ClientPolicyBudget budget)
        {
            var data = _hanysContext.ClientPolicieBudgets.FirstOrDefault(c => c.ClientId == budget.ClientId
            && c.PolicyId == budget.PolicyId
            && c.BudgetYear == budget.BudgetYear
            && c.BudgetMonth == budget.BudgetMonth);
            if (data != null)
            {
                data.IsActive = budget.IsActive;
                data.BudgetValue = budget.BudgetValue;
                data.RevDate = budget.RevDate;
                data.RevUser = budget.RevUser;
                _hanysContext.SaveChanges();
            }
            return true;
        }

        public IList<ClientPolicyBudget> GetClientPolicyBudgetByClientYear(int clientId, int year)
        {
            return _hanysContext.ClientPolicieBudgets.Where(cpb => cpb.ClientId == clientId && cpb.BudgetYear == year).ToList();
        }

       

        public IList<ClientPolicyBudget> GetAllPolicyBudgetsForClientPolicyYear(int clientId, int policyId, int year)
        {
            //var budget = _hanysContext.ClientPolicieBudgets.Where(b => b.ClientId == clientId
            //  && b.PolicyId == policyId
            //  && b.BudgetYear == year).ToList();

            //return budget;
            var budgets = _hanysContext.ClientPolicieBudgets.ToList();
           // budgets.Select(MapTrnsactions);
            if (clientId > 0)
            {
                budgets = budgets.Where(b => b.ClientId == clientId).ToList();
            }
            if (year > 0)
            {
                budgets = budgets.Where(b => b.BudgetYear == year).ToList();
            }

            return budgets;
        }

        public ClientPolicie GetClientPoliciesByPolicyId(int policyId)
        {
            return _hanysContext.ClientPolicies.FirstOrDefault(cp => cp.PolicieId == policyId);
        }

        public IList<CorporateProduct> GetAllCorporateProducts()
        {
            return _hanysContext.CorporateProduct.ToList();
        }

        public IList<CorporateProduct> GetAllCorporateProductsByCoverageId(int coverageId)
        {
            return _hanysContext.CorporateProduct.Where(cp => cp.CoverageId == coverageId).ToList();
        }

        public CorporateProduct GetCorporateProductById(int productId)
        {
            return _hanysContext.CorporateProduct.FirstOrDefault(cp => cp.Id == productId);
        }


        IList<CorporateProductsXProduct> ICommonRepository.GetAllCorporateXProducts()
        {
            return _hanysContext.CorporateProductsXProducts.ToList();
        }

        public bool DeleteCommission(int clientPolicyId)
        {
            var commission = _hanysContext.Commisions.FirstOrDefault(c => c.ClientPolicyId == clientPolicyId);
            _hanysContext.Commisions.Attach(commission);
            _hanysContext.Commisions.Remove(commission);
            _hanysContext.SaveChanges();

            return true;
        }
        public IList<Client> GetAllClients()
        {
            return _hanysContext.Clients.ToList();
        }
    }
}