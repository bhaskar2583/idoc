﻿using IBS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBS.Service.Repositories
{
    public interface ICommonRepository
    {
        bool UpdateCoverages(Coverage coverage);
        bool UpdateProduct(Product product);
        bool AddCoverages(Coverage coverage);
        bool AddProduct(Product product);
        IList<ClientPolicie> GetAllClientPoliciesByIndexId(int id);
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
        IList<ClientPolicyBudget> GetClientPolicyBudgetByClientYear(int clientId, int year);
        bool AddClientPolocyBudget(ClientPolicyBudget budget);
        bool UpdateClientPolocyBudget(ClientPolicyBudget budget);
        IList<ClientPolicyBudget> GetAllPolicyBudgetsForClientPolicyYear(int clientId, int policyId, int year);
        ClientPolicyBudget GetAllPolicyBudgetsForPolicyYearMonth(int policyId, int year,string month);

        ClientPolicie GetClientPoliciesByPolicyId(int policyId);


        IList<CorporateProduct> GetAllCorporateProducts();
        IList<CorporateProduct> GetAllCorporateProductsByCoverageId(int coverageId);
        CorporateProduct GetCorporateProductById(int productId);

        IList<CorporateProductsXProduct> GetAllCorporateXProducts();
        bool DeleteCommission(int clientPolicyId);
        IList<Client> GetAllClients();

    }
}