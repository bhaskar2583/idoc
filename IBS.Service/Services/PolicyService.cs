using IBS.Core.Entities;
using IBS.Core.Enums;
using IBS.Core.Models;
using IBS.Service.Repositories;
using IBS.Service.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBS.Service.Services
{
    public class PolicyService : IPolicyService
    {
        private readonly IPolicyRepository _policyRepository;
        private readonly ICarrierService _carrierService;
        private readonly ICommonService _commonService;
       // private readonly IClientService _clientService;
        public PolicyService(IPolicyRepository policyRepository, ICarrierService carrierService,
            ICommonService commonService//, IClientService clientService
            )
        {
            _policyRepository = policyRepository;
            _carrierService = carrierService;
            _commonService = commonService;
         //   _clientService = clientService;
        }

        public bool AddPolicy(PolicyModel policy)
        {
            var entity = new Policie()
            {
                PolicyNumber = policy.PolicyNumber,
                PolicyType = policy.PolicyType,
                CarId = policy.CarId,
                EffectiveDate = (DateTime)policy.EffectiveDate,
                EndDate = (DateTime)policy.EndDate,
                IsGroupInsurance = policy.IsGroupInsurance,
                IsActive = true,
                AddUser = LoginUserDetails.GetWindowsLoginUserName(),
                AddDate = DateUtil.GetCurrentDate(),
                CoverageId=policy.CoverageId,
                ProductId=policy.ProductId
            };

            return _policyRepository.Add(entity);
        }

        public bool DeletePolicy(int policyId)
        {
            return _policyRepository.Delete(policyId);
        }

        public IList<PolicyModel> GetAllPolicies()
        {
            var policies = new List<PolicyModel>();

            var policiesData = _policyRepository.GetAll().ToList();

            if (policiesData == null)
            {
                return policies;
            }

            var carriers = _carrierService.GetAllCarriers();
            var coverages = _commonService.GetAllCoverages();
            var products = _commonService.GetAllProducts();
            policiesData.ForEach(c =>
            {
                var policy = new PolicyModel()
                {
                    Id = c.Id,
                    IsActive = (bool)c.IsActive,
                    PolicyNumber = c.PolicyNumber,
                    PolicyType = c.PolicyType,
                    CarId = c.CarId,
                    CoverageId=c.CoverageId,
                    ProductId=c.ProductId,
                    EffectiveDate = c.EffectiveDate,
                    EndDate = c.EndDate,
                    IsGroupInsurance = (bool)c.IsGroupInsurance,
                    AddUser = c.AddUser,
                    AddDate = c.AddDate,
                    RevUser = c.RevUser,
                    RevDate = c.RevDate
                };

                MapSelectedCarrier(policy, carriers?.FirstOrDefault(cr => cr.Id == policy.CarId));
                MapSelectedCoverage(policy, coverages?.FirstOrDefault(cov => cov.Id == policy.CoverageId));
                MapSelectedProduct(policy, products?.FirstOrDefault(pro => pro.Id == policy.ProductId));
                policies.Add(policy);
            });

            return policies;
        }

        private void MapSelectedCarrier(PolicyModel policy, CarrierModel carrier)
        {
            if (carrier == null)
                return;
            policy.SelectedCarrier = new CarrierDdlModel()
            {
                Id = carrier.Id,
                Name = carrier.Name
            };
        }

        private void MapSelectedCoverage(PolicyModel policy, Coverage coverage)
        {
            if (coverage == null)
                return;
            policy.SelectedCoverage = new Coverage()
            {
                Id = coverage.Id,
                Name = coverage.Name
            };
        }

        private void MapSelectedProduct(PolicyModel policy, Product product)
        {
            if (product == null)
                return;
            policy.SelectedProduct = new Product()
            {
                Id = product.Id,
                Name = product.Name
            };
        }


        public PolicyModel GetById(int Id)
        {
            var entity = _policyRepository.GetById(Id);

            if (entity != null)
            {
                var policy= new PolicyModel()
                {
                    Id = entity.Id,
                    IsActive = (bool)entity.IsActive,
                    PolicyNumber = entity.PolicyNumber,
                    PolicyType = entity.PolicyType,
                    CarId = entity.CarId,
                    CoverageId=entity.CoverageId,
                    ProductId=entity.ProductId,
                    EffectiveDate = entity.EffectiveDate,
                    EndDate = entity.EndDate,
                    IsGroupInsurance = (bool)entity.IsGroupInsurance,
                    AddUser = entity.AddUser,
                    AddDate = entity.AddDate,
                    RevUser = entity.RevUser,
                    RevDate = entity.RevDate
                };

                var carrier = _carrierService.GetById(policy.CarId);
                var coverage = _commonService.GetCoverageById(policy.CoverageId);
                var product = _commonService.GetProductById(policy.ProductId);
                MapSelectedCarrier(policy, carrier);
                MapSelectedProduct(policy, product);
                MapSelectedCoverage(policy, coverage);
                return policy;
            }

            return null;
        }

        public bool ModifyPolicy(PolicyModel policy)
        {
            var entity = new Policie()
            {
                Id = policy.Id,
                PolicyNumber = policy.PolicyNumber,
                IsActive = policy.IsActive,
                PolicyType = policy.PolicyType,
                CarId = policy.CarId,
                CoverageId=policy.CoverageId,
                ProductId=policy.ProductId,
                EffectiveDate = (DateTime)policy.EffectiveDate,
                EndDate = (DateTime)policy.EndDate,
                IsGroupInsurance = policy.IsGroupInsurance,
                AddUser = policy.AddUser,
                AddDate = policy.AddDate,
                RevDate = DateUtil.GetCurrentDate(),
                RevUser = LoginUserDetails.GetWindowsLoginUserName()
            };

            return _policyRepository.Update(entity);
        }

        public void MapCarriers(PolicyModel policy)
        {
            var carriers = _carrierService.GetAllCarriers();
            carriers.ToList().ForEach(c =>
            {
                policy.Carriers.Add(new CarrierDdlModel()
                {
                    Id = c.Id,
                    Name = c.Name
                });
            });
        }

        public void MapProductsOfCoverage(PolicyModel policy,int coverageId)
        {
            var products = _commonService.GetAllProductsByCoverageId(coverageId);
            products.ToList().ForEach(c =>
            {
                policy.Products.Add(new Product()
                {
                    Id = c.Id,
                    Name = c.Name
                });
            });
        }

        public void MapCoverages(PolicyModel policy)
        {
            var coverages = _commonService.GetAllCoverages();
            coverages.ToList().ForEach(c =>
            {
                policy.Coverages.Add(new Coverage()
                {
                    Id = c.Id,
                    Name = c.Name
                });
            });
        }

        public IList<PolicyModel> ApplyFilterForIndex(string policyNumber, CarrierStatusEnum searchStatus, IList<PolicyModel> source)
        {
            if (source == null)
                return null;

            if (!string.IsNullOrEmpty(policyNumber))
            {
                source = source.Where(c => c.PolicyNumber.ToLower().Contains(policyNumber)).ToList();
            }

            switch (searchStatus)
            {
                case CarrierStatusEnum.Active:
                    source = source.Where(c => c.IsActive == true).ToList();
                    break;
                case CarrierStatusEnum.InActive:
                    source = source.Where(c => c.IsActive == false).ToList();
                    break;
                case CarrierStatusEnum.None:
                    break;
            }
            return source;
        }

        public IList<ClientPolicyBudget> GetAllPolicyBudgets(int policyId)
        {
            throw new NotImplementedException();
        }
        public IList<PolicyBudgetsModel> GetAllPolicyBudgetsForClientPolicyYear(int clientId, int policyId, int year)
        {
            var policyBudget = new List<PolicyBudgetsModel>();
            var budgetDetails = _commonService.GetAllPolicyBudgetsForClientPolicyYear(clientId, policyId, year);

            if (budgetDetails == null)
                return policyBudget;

            budgetDetails.ToList().ForEach(b =>
            {
                var isExist = policyBudget.FirstOrDefault(pb => pb.Year == b.BudgetYear
                && pb.PolicyId == b.PolicyId);

                if (isExist == null)
                {
                    var policy = _policyRepository.GetById(b.PolicyId);
                    var budget = new PolicyBudgetsModel()
                    {
                        PolicyId = b.PolicyId,
                        PolicyNumber = policy.PolicyNumber,
                        ClientId = b.ClientId,
                        Year = b.BudgetYear
                    };
                    policyBudget.Add(budget);
                    AssignBudget(budget, b.BudgetMonth, b.BudgetValue);
                }
                if (isExist != null)
                {
                    AssignBudget(isExist, b.BudgetMonth, b.BudgetValue);
                }
            });
            return policyBudget;
        }
        IList<PolicyBudgetsModel> IPolicyService.GetAllPolicyBudgets(int policyId)
        {
            var policyBudget = new List<PolicyBudgetsModel>();
            var budgetDetails = _commonService.GetAllPolicyBudgets(policyId);

            if (budgetDetails == null)
                return policyBudget;

            budgetDetails.ToList().ForEach(b =>
            {
                var isExist = policyBudget.FirstOrDefault(pb => pb.Year == b.BudgetYear
                && pb.PolicyId == b.PolicyId);

                if (isExist == null)
                {
                    var policy = _policyRepository.GetById(b.PolicyId);
                    var budget = new PolicyBudgetsModel()
                    {
                        PolicyId = b.PolicyId,
                        PolicyNumber = policy.PolicyNumber,
                        ClientId=b.ClientId,
                        Year = b.BudgetYear
                    };
                    policyBudget.Add(budget);
                    AssignBudget(budget, b.BudgetMonth, b.BudgetValue);
                }
                if (isExist != null)
                {
                    AssignBudget(isExist, b.BudgetMonth, b.BudgetValue);
                }
            });
            return policyBudget;
        }

        private void AssignBudget(PolicyBudgetsModel budget,string month,decimal amount)
        {
            switch (month.ToLower())
            {
                case "jan" :
                    budget.JanBudget = amount;
                    break;
                case "feb":
                    budget.FebBudget = amount;
                    break;
                case "mar":
                    budget.MarchBudget = amount;
                    break;
                case "apr":
                    budget.AprilBudget = amount;
                    break;
                case "may":
                    budget.MayBudget = amount;
                    break;
                case "jun":
                    budget.JunBudget = amount;
                    break;
                case "jul":
                    budget.JulyBudget = amount;
                    break;
                case "aug":
                    budget.AugBudget = amount;
                    break;
                case "sep":
                    budget.SepBudget = amount;
                    break;
                case "oct":
                    budget.OctBudget = amount;
                    break;
                case "nov":
                    budget.NovBudget = amount;
                    break;
                case "dec":
                    budget.DecBudget = amount;
                    break;

            }
        }
    }
}
