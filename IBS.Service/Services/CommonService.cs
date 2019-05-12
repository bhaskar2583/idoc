﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBS.Core.Entities;
using IBS.Core.Models;
using IBS.Service.Repositories;
using IBS.Service.Utils;

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

        public IList<ClientPolicyBudget> GetAllPolicyBudgets(int policyId)
        {
            return _commonRepository.GetAllPolicyBudgets(policyId);
        }

        public IList<Product> GetAllProductsByCoverageId(int coverageId)
        {
            return _commonRepository.GetAllProductsByCoverageId(coverageId);
        }

        public bool AddClientPolocyBudget(AddPolicyBudget budget)
        {
            

            foreach (var month in DateUtil.GetMonths())
            {
                var entity = new ClientPolicyBudget()
                {
                    ClientId = budget.ClientId,
                    PolicyId= budget.PolicyId,
                    ClientPolicyId=budget.ClientPolicyId,
                    IsActive = true,
                    AddUser = LoginUserDetails.GetWindowsLoginUserName(),
                    AddDate = DateUtil.GetCurrentDate(),
                    BudgetYear = budget.Year,
                    BudgetMonth = month
                };

                switch (entity.BudgetMonth.ToLower())
                {
                    case "jan":
                        entity.BudgetValue = budget.JanBudget;
                        break;
                    case "feb":
                        entity.BudgetValue = budget.FebBudget;
                        break;
                    case "mar":
                        entity.BudgetValue = budget.MarchBudget;
                        break;
                    case "apr":
                        entity.BudgetValue = budget.AprilBudget;
                        break;
                    case "may":
                        entity.BudgetValue = budget.MayBudget;
                        break;
                    case "jun":
                        entity.BudgetValue = budget.JunBudget;
                        break;
                    case "jul":
                        entity.BudgetValue = budget.JulyBudget;
                        break;
                    case "aug":
                        entity.BudgetValue = budget.AugBudget;
                        break;
                    case "sep":
                        entity.BudgetValue = budget.SepBudget;
                        break;
                    case "oct":
                        entity.BudgetValue = budget.OctBudget;
                        break;
                    case "nov":
                        entity.BudgetValue = budget.NovBudget;
                        break;
                    case "dec":
                        entity.BudgetValue = budget.DecBudget;
                        break;
                }

                _commonRepository.AddClientPolocyBudget(entity);
            }
            return true;
        }

        public bool UpdateClientPolocyBudget(AddPolicyBudget budget)
        {
            var budgets = _commonRepository.GetAllPolicyBudgetsForClientPolicyYear(budget.ClientId, budget.PolicyId,budget.Year);
            budgets.ToList().ForEach(b =>
            {
                switch (b.BudgetMonth.ToLower())
                {
                    case "jan":
                        b.BudgetValue = budget.JanBudget;
                        break;
                    case "feb":
                        b.BudgetValue = budget.FebBudget;
                        break;
                    case "mar":
                        b.BudgetValue = budget.MarchBudget;
                        break;
                    case "apr":
                        b.BudgetValue = budget.AprilBudget;
                        break;
                    case "may":
                        b.BudgetValue = budget.MayBudget;
                        break;
                    case "jun":
                        b.BudgetValue = budget.JunBudget;
                        break;
                    case "jul":
                        b.BudgetValue = budget.JulyBudget;
                        break;
                    case "aug":
                        b.BudgetValue = budget.AugBudget;
                        break;
                    case "sep":
                        b.BudgetValue = budget.SepBudget;
                        break;
                    case "oct":
                        b.BudgetValue = budget.OctBudget;
                        break;
                    case "nov":
                        b.BudgetValue = budget.NovBudget;
                        break;
                    case "dec":
                        b.BudgetValue = budget.DecBudget;
                        break;
                }

                _commonRepository.UpdateClientPolocyBudget(b);
            });

            return true;
        }

        public IList<ClientPolicyBudget> GetAllPolicyBudgetsForClientPolicyYear(int clientId, int policyId, int year)
        {
           
            return _commonRepository.GetAllPolicyBudgetsForClientPolicyYear(clientId, policyId, year);
           
        }
        public ClientPolicie GetClientPoliciesByPolicyId(int policyId)
        {
            return _commonRepository.GetClientPoliciesByPolicyId(policyId);
        }

        public IList<CorporateProductsXProduct> GetAllCorporateXProducts()
        {
            return _commonRepository.GetAllCorporateXProducts();
        }
    }
}
