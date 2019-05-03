using IBS.Core.Entities;
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
    public class CommisionService : ICommisionService
    {
        private readonly ICommonRepository _commonRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IPolicyRepository _policyRepository;
        private readonly ICommisionRepository _commissionRepository;
        public CommisionService(ICommonRepository commonRepository, IClientRepository clientRepository, 
            IPolicyRepository policyRepository, ICommisionRepository commissionRepository)
        {
            _commonRepository = commonRepository;
            _clientRepository = clientRepository;
            _policyRepository = policyRepository;
            _commissionRepository = commissionRepository;
        }
        public List<CommisionModel> GetCarrierPoliciesById(int carrierId)
        {
            var commisions = new List<CommisionModel>();

            var clientPolicies = _commonRepository.GetAllClientPolicies().Where(cp=>cp.IsActive);
            clientPolicies.ToList().ForEach(cp =>
            {
                
                var policyDetails = _policyRepository.GetById(cp.PolicieId);
                
                if(policyDetails.CarId== carrierId)
                {
                    var commisionMode = new CommisionModel();
                    var product = _commonRepository.GetProductById(policyDetails.ProductId);
                    var coverage = _commonRepository.GetCoverageById(policyDetails.CoverageId);
                    var clientDetails = _clientRepository.GetById(cp.ClientId);

                    commisionMode.CoverageId = coverage.Id;
                    commisionMode.CoverageName = coverage.Name;

                    commisionMode.PolicyId = policyDetails.Id;
                    commisionMode.PolicyNumber = policyDetails.PolicyNumber;

                    commisionMode.ProductId = product.Id;
                    commisionMode.ProductName = product.Name;

                    commisionMode.ClientId = clientDetails.Id;
                    commisionMode.CleintName = clientDetails.Name;

                    commisionMode.DivisionId =Convert.ToInt32(clientDetails.Division);
                    commisionMode.DivisionName = commisionMode.DivisionId == 1 ? "Division 1" : "Division 2";

                    commisionMode.ClientPolicyId = cp.Id;
                    commisionMode.CarrierId = carrierId;

                    //var commision = _commissionRepository.GetByClientPolicyId(commisionMode.ClientPolicyId);

                    //if (commision != null)
                    //{
                    //    commisionMode.Status = commision.Status;
                    //    commisionMode.CommisionValue = commision.CommisionValue;
                    //    commisionMode.StatementDate = commision.StatementDate;
                    //    commisionMode.AppliedDate = commision.AppliedDate;
                    //    commisionMode.PaymentId = commision.PaymentId;

                    //    commisionMode.SelectedCoverage = new Coverage()
                    //    {
                    //        Id = coverage.Id,
                    //        Name = coverage.Name
                    //    };

                    //    commisionMode.SelectedProduct = new Product()
                    //    {
                    //        Id=product.Id,
                    //        Name=product.Name
                    //    };
                    //    var isExist = commisions.FirstOrDefault(c => c.ClientId == commisionMode.ClientId
                    //    && c.PolicyNumber == commisionMode.PolicyNumber);
                    //    if (isExist != null)
                    //    {
                    //        isExist.Coverages.Add(new Coverage()
                    //        {
                    //            Id = commisionMode.CoverageId,
                    //            Name = commisionMode.CoverageName
                    //        });
                    //        commisionMode.Coverages = isExist.Coverages;
                    //    }
                    //    if (isExist == null)
                    //    {
                    //        commisionMode.Coverages.Add(new Coverage()
                    //        {
                    //            Id = commisionMode.CoverageId,
                    //            Name = commisionMode.CoverageName
                    //        });
                    //    }

                    //    commisions.Add(commisionMode);
                    //}
                    //else
                    //{
                    //    var isExist = commisions.FirstOrDefault(c => c.ClientId == commisionMode.ClientId 
                    //    && c.PolicyNumber == commisionMode.PolicyNumber);
                    //    if (isExist!=null)
                    //    {
                    //        isExist.Coverages.Add(new Coverage()
                    //        {
                    //            Id = commisionMode.CoverageId,
                    //            Name = commisionMode.CoverageName
                    //        });
                    //    }
                    //    if (isExist == null)
                    //    {
                    //        commisionMode.Coverages.Add(new Coverage()
                    //        {
                    //            Id = commisionMode.CoverageId,
                    //            Name = commisionMode.CoverageName
                    //        });
                    //        commisions.Add(commisionMode);
                    //    }
                        
                    //}

                    var isExist = commisions.FirstOrDefault(c => c.ClientId == commisionMode.ClientId
                        && c.PolicyNumber == commisionMode.PolicyNumber);
                    if (isExist != null)
                    {
                        isExist.Coverages.Add(new Coverage()
                        {
                            Id = commisionMode.CoverageId,
                            Name = commisionMode.CoverageName
                        });
                    }
                    if (isExist == null)
                    {
                        commisionMode.Coverages.Add(new Coverage()
                        {
                            Id = commisionMode.CoverageId,
                            Name = commisionMode.CoverageName
                        });
                        commisions.Add(commisionMode);
                    }

                }
            });
            //Product management
            commisions.ForEach(c =>
            {
                if (c.SelectedCoverage != null && c.SelectedCoverage.Id > 0)
                {
                    //var policy = _policyRepository.GetById(c.PolicyId);
                    //var products = _policyRepository.GetAll().Where(p => p.PolicyNumber == policy.PolicyNumber && p.CoverageId == c.SelectedCoverage.Id);
                    //products.ToList().ForEach(p =>
                    //{
                    //    c.Products.Add(new Product()
                    //    {
                    //        Id = p.ProductId,
                    //        Name = _commonRepository.GetAllProducts().FirstOrDefault(pro => pro.Id == p.ProductId).Name
                    //    });
                    //});

                    var products = GetProductsOfPolicy(Convert.ToString(c.ClientId), c.PolicyNumber,Convert.ToString(c.CoverageId));
                    c.Products = products;
                }
                //else
                //{
                //    var coverage = c.Coverages.FirstOrDefault();
                //    if (coverage != null)
                //    {
                //        var policy = _policyRepository.GetById(c.PolicyId);
                //        var products = _policyRepository.GetAll().Where(p => p.PolicyNumber == policy.PolicyNumber && p.CoverageId == coverage.Id);
                //        products.ToList().ForEach(p =>
                //        {
                //            c.Products.Add(new Product()
                //            {
                //                Id = p.ProductId,
                //                Name = _commonRepository.GetAllProducts().FirstOrDefault(pro => pro.Id == p.ProductId).Name
                //            });
                //        });
                //    }
                //}
            });
            return commisions;
        }

        public ClientPolicie GetClientPoliciesByPolicyId(int policyId)
        {
            return _commonRepository.GetClientPoliciesByPolicyId(policyId);
        }

        public Policie GetPolicyByNoCarriageCoverage(string policyNo, int carrierId, int coverageId,int product)
        {
            return _policyRepository.GetPolicyByNoCarriageCoverage(policyNo, carrierId, coverageId,product);
        }

        public List<Product> GetProductsOfPolicy(string client, string policyNo, string coverage)
        {
            var products = new List<Product>();
            var clientPolicies = _commonRepository.GetAllClientPolicies().Where(cp => cp.IsActive 
            && cp.ClientId==Convert.ToInt32(client)).ToList();

            clientPolicies.ForEach(cp => {
                var policyDetails = _policyRepository.GetById(cp.PolicieId);

                if(policyDetails.PolicyNumber==policyNo && Convert.ToString(policyDetails.CoverageId) == coverage)
                {
                    var isExist = products.FirstOrDefault(pr => pr.Id == policyDetails.ProductId);
                    if (isExist == null)
                    {
                        products.Add(new Product()
                        {
                            Id = policyDetails.ProductId,
                            Name = _commonRepository.GetAllProducts().FirstOrDefault(pro => pro.Id == policyDetails.ProductId).Name
                        });
                    }
                }
                
            });
            return products;
            
        }

        public bool SaveCommisions(List<CommisionModel> commissions)
        {
            commissions.ForEach(c =>
            {
                var isExist = _commissionRepository.GetByClientPolicyId(c.ClientPolicyId);
                if (isExist != null)
                {
                    c.RevUser = LoginUserDetails.GetWindowsLoginUserName();
                    c.RevDate = DateUtil.GetCurrentDate();
                    _commissionRepository.Update(c);
                    return;
                }
                var commission = new Commision()
                {
                    ClientPolicyId = c.ClientPolicyId,
                    CommisionValue = c.CommisionValue,
                    AppliedDate = c.AppliedDate,
                    StatementDate = c.StatementDate,
                    PaymentId = c.PaymentId,
                    AddUser = LoginUserDetails.GetWindowsLoginUserName(),
                    AddDate = DateUtil.GetCurrentDate(),
                    Status=c.Status
                };
                _commissionRepository.Add(commission);
            });
            return true;
        }
    }
}
