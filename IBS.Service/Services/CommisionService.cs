﻿using IBS.Core.Entities;
using IBS.Core.Models;
using IBS.Service.Repositories;
using IBS.Service.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
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
        private readonly ICarrierRepository _carrierRepository;
        public CommisionService(ICommonRepository commonRepository, IClientRepository clientRepository,
            IPolicyRepository policyRepository, ICommisionRepository commissionRepository, ICarrierRepository carrierRepository)
        {
            _commonRepository = commonRepository;
            _clientRepository = clientRepository;
            _policyRepository = policyRepository;
            _commissionRepository = commissionRepository;
            _carrierRepository = carrierRepository;
        }
        private string GetDateFormat(DateTime? date)
        {
            if (date == null)
                return string.Empty;

            DateTime dt = Convert.ToDateTime(date);
            return dt.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
        }

        private string GetDateFormatMMYYYY(DateTime? date)
        {
            if (date == null)
                return string.Empty;

            DateTime dt = Convert.ToDateTime(date);
            var dateAsMMDD = dt.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);

            string[] dateArray = dateAsMMDD.Split('/');
            dateAsMMDD = dateArray[0] + "/" + dateArray[2];
            return dateAsMMDD;
        }

        public ClientPolicie GetAllClientPoliciesByIndexId(int Id)
        {
          return  _commonRepository.GetAllClientPoliciesByIndexId(Id).FirstOrDefault();
        }
        public List<Commision> GetSavedCommisions()
        {
            return _commissionRepository.GetSavedCommisions();
        }
        public List<CommisionModel> GetAllSavedCommissionsForCarrier(int carrierId)
        {
            var commissionModel = new List<CommisionModel>();
            var commissions = _commissionRepository.GetSavedCommisionsForCarrier(carrierId);
            commissions.ForEach(dc =>
            {
                var cpDetails = _commonRepository.GetAllClientPoliciesByIndexId(dc.ClientPolicyId).FirstOrDefault();
                var policyDetails = _policyRepository.GetById(cpDetails.PolicieId);
                var product = _commonRepository.GetProductById(policyDetails.ProductId);
                var coverage = _commonRepository.GetCoverageById(policyDetails.CoverageId);
                var clientDetails = _clientRepository.GetById(cpDetails.ClientId);
                var carrier = _carrierRepository.GetById(policyDetails.CarId);

                commissionModel.Add(new CommisionModel()
                {
                    Id = dc.Id,
                    ReconcilationPaymentDate = dc.ReconcilationDate,
                    ReconcilationPaymentDateAsString = GetDateFormat(dc.ReconcilationDate),
                    ReconsilationStatus = string.IsNullOrEmpty(dc.ReconcilationStatus) ? "Open" : "Verified",
                    ReconsilationStatusId = string.IsNullOrEmpty(dc.ReconcilationStatus) ? 1 : 2,
                    ClientId = clientDetails.Id,
                    CoverageId = coverage.Id,
                    CoverageName = coverage.Name,

                    PolicyId = policyDetails.Id,
                    PolicyNumber = policyDetails.PolicyNumber,

                    ProductId = product.Id,
                    ProductName = product.Name,

                    CleintName = clientDetails.Name,


                    ClientPolicyId = dc.ClientPolicyId,
                    CarrierId = policyDetails.CarId,
                    CarrierName = carrier.Name,
                    CommisionValue = dc.CommisionValue,
                    CommisionString = dc.CommisionString,
                    StatementDateAsString = GetDateFormat(dc.StatementDate),
                    StatementDate=dc.StatementDate,
                    AppliedDate = dc.AppliedDate,
                    AppliedDateAsString = GetDateFormatMMYYYY(dc.AppliedDate),
                    AppliedDateAsFullString = GetDateFormat(dc.AppliedDate),
                    PaymentId = dc.PaymentId,
                    DivisionName = clientDetails.Division,
                    Git=policyDetails.IsGroupInsurance
                    
                });
            });

            commissionModel.ForEach(cm =>
            {

                if (!string.IsNullOrEmpty(cm.DivisionName))
                {
                    if (cm.DivisionName == "HBS")
                    {
                        cm.DivisionId = 1;
                    }
                    if (cm.DivisionName == "SBS")
                    {
                        cm.DivisionId = 2;
                    }
                }

                var carrierProduct = _commonRepository.GetAllCorporateXProducts().FirstOrDefault(cp => cp.ProductId == cm.ProductId);
                if (carrierProduct != null)
                {
                    var cp = _commonRepository.GetAllCorporateProducts().FirstOrDefault(cpp => cpp.Id == carrierProduct.CorporateProductId);
                    if (cp != null)
                    {
                        cm.SelectedCorporateProduct = new CorporateProduct()
                        {
                            Id = cp.Id,
                            Name = cp.Name
                        };
                    }
                }

            });
            return commissionModel;
        }

        public List<CommisionModel> GetCarrierPoliciesById(int carrierId)
        {
            var commisions = new List<CommisionModel>();

            var clientPolicies = _commonRepository.GetAllClientPolicies().Where(cp => cp.IsActive);
            clientPolicies.ToList().ForEach(cp =>
            {

                var policyDetails = _policyRepository.GetById(cp.PolicieId);

                if (policyDetails.CarId == carrierId)
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

                    if (!string.IsNullOrEmpty(clientDetails.Division))
                    {
                        if (clientDetails.Division == "HBS")
                        {
                            commisionMode.DivisionId = 1;
                            commisionMode.DivisionName = clientDetails.Division;
                        }
                        if (clientDetails.Division == "SBS")
                        {
                            commisionMode.DivisionId = 2;
                            commisionMode.DivisionName = clientDetails.Division;
                        }
                    }
                    commisionMode.Git = policyDetails.IsGroupInsurance;
                    //commisionMode.DivisionId = Convert.ToInt32(clientDetails.Division);
                    //commisionMode.DivisionName = commisionMode.DivisionId == 1 ? "Division 1" : "Division 2";

                    commisionMode.ClientPolicyId = cp.Id;
                    commisionMode.CarrierId = carrierId;



                    var isExist = commisions.FirstOrDefault(c => c.ClientId == commisionMode.ClientId
                        && c.PolicyNumber == commisionMode.PolicyNumber);
                    if (isExist != null)
                    {
                        isExist.Products.Add(new Product()
                        {
                            Id = commisionMode.ProductId,
                            Name = commisionMode.ProductName
                        });
                    }
                    if (isExist == null)
                    {
                        commisionMode.Products.Add(new Product()
                        {
                            Id = commisionMode.ProductId,
                            Name = commisionMode.ProductName
                        });
                        commisions.Add(commisionMode);
                    }

                }
            });
            //Product management
            commisions.ForEach(c =>
            {
                c.Products.ForEach(cp =>
                {
                    var cpProduct = _commonRepository.GetAllCorporateXProducts().FirstOrDefault(cxp => cxp.ProductId == cp.Id);
                    if (cpProduct != null)
                    {
                        var cProduct = _commonRepository.GetAllCorporateProducts().FirstOrDefault(p => p.Id == cpProduct.CorporateProductId);
                        if (cProduct != null)
                        {
                            var isExit = c.CorporateProducts.FirstOrDefault(cpp => cpp.Id == cProduct.Id);
                            if (isExit == null)
                            {
                                c.CorporateProducts.Add(cProduct);
                            }
                        }
                    }
                });
                //if (c.SelectedCoverage != null && c.SelectedCoverage.Id > 0)
                //{

                //var products = GetProductsOfPolicy(Convert.ToString(c.ClientId), c.PolicyNumber, Convert.ToString(c.CoverageId));
                //    c.Products = products;
                //}

            });
            return commisions;
        }

        public ClientPolicie GetClientPoliciesByPolicyId(int policyId)
        {
            return _commonRepository.GetClientPoliciesByPolicyId(policyId);
        }

        public Policie GetPolicyByNoCarriageCoverage(string policyNo, int carrierId, int coverageId, int product)
        {
            return _policyRepository.GetPolicyByNoCarriageCoverage(policyNo, carrierId, coverageId, product);
        }

        public List<Product> GetProductsOfPolicy(string client, string policyNo, string coverage)
        {
            var products = new List<Product>();
            var clientPolicies = _commonRepository.GetAllClientPolicies().Where(cp => cp.IsActive
            && cp.ClientId == Convert.ToInt32(client)).ToList();

            clientPolicies.ForEach(cp =>
            {
                var policyDetails = _policyRepository.GetById(cp.PolicieId);

                if (policyDetails.PolicyNumber == policyNo && Convert.ToString(policyDetails.CoverageId) == coverage)
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


        public bool SaveCommisions(List<CommisionModel> commissions, bool save)
        {
            commissions.ForEach(c =>
            {
                if (save)
                {
                    var isExist = _commissionRepository.GetByClientPolicyId(c.ClientPolicyId);
                    if (isExist != null)
                    {
                        c.RevUser = LoginUserDetails.GetWindowsLoginUserName();
                        c.RevDate = DateUtil.GetCurrentDate();
                        _commissionRepository.Update(c);
                        return;
                    }
                }

                var commission = new Commision()
                {
                    ClientPolicyId = c.ClientPolicyId,
                    CommisionValue = c.CommisionValue,
                    CommisionString=c.CommisionString,
                    AppliedDate = c.AppliedDate,
                    StatementDate = c.StatementDate,
                    PaymentId = c.PaymentId,
                    AddUser = LoginUserDetails.GetWindowsLoginUserName(),
                    AddDate = DateUtil.GetCurrentDate(),
                    Status = c.Status,
                    IsExceptionCommission=c.IsExceptionCommission
                };
                _commissionRepository.Add(commission);
            });
            return true;
        }

        public bool UpdateCommisions(List<CommisionModel> commissions)
        {
            commissions.ForEach(c =>
            {
                var isExist = _commissionRepository.GetByClientPolicyId(c.ClientPolicyId);
                if (isExist != null)
                {
                    c.ReconsilationStatus = "Verified";
                    c.RevUser = LoginUserDetails.GetWindowsLoginUserName();
                    c.RevDate = DateUtil.GetCurrentDate();
                    _commissionRepository.UpdateCommissionReconsilation(c);
                    return;
                }

            });
            return true;
        }

        public List<SelectListCommon> GetCarrierStatementDates(string carrierId)
        {
            var smDates = new List<SelectListCommon>();
            var clientPolicies = _commonRepository.GetAllClientPolicies().Where(cp => cp.IsActive);
            clientPolicies.ToList().ForEach(cp =>
            {

                var policyDetails = _policyRepository.GetById(cp.PolicieId);

                if (policyDetails.CarId == Convert.ToInt32(carrierId))
                {
                    var commisions = _commissionRepository.GetByAllClientPolicyId(cp.Id);

                    if (commisions != null)
                    {
                        commisions.ForEach(c =>
                        {
                            var sDate = GetDateFormat(c.StatementDate);

                            var isExist = smDates.FirstOrDefault(s => s.Name == sDate);
                            if (isExist == null)
                            {
                                smDates.Add(new SelectListCommon()
                                {
                                    Id = c.Id,
                                    Name = GetDateFormat(c.StatementDate)
                                });
                            }
                           
                        });

                    }
                }
            });

            return smDates;
        }

        public List<SelectListCommon> GetCarrierStatementDatePayments(string carrierId, string statementDate)
        {
            var payments = new List<SelectListCommon>();
            var clientPolicies = _commonRepository.GetAllClientPolicies().Where(cp => cp.IsActive);
            clientPolicies.ToList().ForEach(cp =>
            {

                var policyDetails = _policyRepository.GetById(cp.PolicieId);

                if (policyDetails.CarId == Convert.ToInt32(carrierId))
                {
                    var commisions = _commissionRepository.GetByAllClientPolicyId(cp.Id);

                    if (commisions != null)
                    {

                        commisions.ForEach(c =>
                        {
                            var smDate = GetDateFormat(c.StatementDate);
                            if (smDate == statementDate)
                            {
                                var isExist = payments.FirstOrDefault(p => p.Name == c.PaymentId);
                                if (isExist == null)
                                {
                                    payments.Add(new SelectListCommon()
                                    {
                                        Id = c.Id,
                                        Name = c.PaymentId
                                    });
                                }
                                
                            }

                        });


                    }
                }
            });

            return payments;
        }

        public IList<CorporateProduct> GetAllCorporateProducts()
        {
            return _commonRepository.GetAllCorporateProducts();
        }

        public Policie GetPolicyByNoCarriageCoverage(string policyNo, int carrierId, int coverageId)
        {
            return _policyRepository.GetAll().
                Where(pc => pc.PolicyNumber == policyNo && pc.CarId == carrierId && pc.CoverageId == coverageId).FirstOrDefault();
        }

        public List<ExceptionCommissionModel> GetAllExceptionCommissionsForCarrier(int? carrierId)
        {
            if (carrierId == null)
                return new List<ExceptionCommissionModel>();

            var x = new CommisionModel();
            var commissionModel = new List<ExceptionCommissionModel>();
            var commissions = _commissionRepository.GetExceptionCommissionsForCarrier((int)carrierId);
            commissions.ForEach(dc =>
            {
                var clientDetails = _clientRepository.GetById(dc.ClientId);
                commissionModel.Add(new ExceptionCommissionModel()
                {
                    Id = dc.Id,
                    ClientId = dc.ClientId,
                    ClientName = dc.ClientId==0?"":clientDetails.Name,
                    PolicyNumber = dc.PolicyNumber,
                    ProductType = dc.ProductType,
                    ClientPolicyId = dc.ClientPolicyId,
                    CarrierId = dc.CarrierId,
                    CommissionValue = dc.CommissionValue,
                    CommissionString = dc.CommissionValue != null ? Convert.ToString(dc.CommissionValue) : null,
                    StatementDateAsString = GetDateFormat(dc.StatementDate),
                    AppliedDate = dc.AppliedDate,
                    AppliedDateAsString = GetDateFormatMMYYYY(dc.AppliedDate),
                    PaymentId = dc.PaymentId,
                    CoverageType=dc.CoverageType,
                    PolicyId=dc.PolicyId
                });
            });
            return commissionModel;
        }

        public IList<Carrier> GetExceptionCommissionsCariers()
        {
            var carriers = new List<Carrier>();
            var exceptionCarriers = _commissionRepository.GetExceptionCommissionsCariers();
            if (exceptionCarriers == null || exceptionCarriers.Count <= 0)
                return carriers;
          
            exceptionCarriers.ForEach(c =>
            {
               var carrier= _carrierRepository.GetById(c);
                if (carrier != null)
                    carriers.Add(carrier);
            });
            return carriers;
        }

        public List<SelectListCommon> GetExceptionCarrierStatementDates(int? carrierId)
        {
            if (carrierId == null)
                return new List<SelectListCommon>();

            return _commissionRepository.GetExceptionCarrierStatementDates((int)carrierId);
        }
        public bool UpdateExceptionCommisions(List<ExceptionCommissionModel> commissions)
        {
            var eCommissions = _commissionRepository.GetExceptionCommissionsForCarrier(0).ToList();

            commissions.ForEach(eC =>
            {
                var eCommission = eCommissions.FirstOrDefault(ec => ec.Id == eC.Id);
                if (eCommission != null)
                {
                    _commissionRepository.UpdateExceptionCommisions(eCommission);
                }
            });
            return true;
        }
        public bool UpdateExceptionCommisionsClient(int Id,int clientId,int policyId,string policyNo)
        {
            var clientPolicies = _commonRepository.GetClientPoliciesByPolicyId(policyId);
            var exceptionCommisions = _commissionRepository.GetExceptionCommissionsByPolicyNo(policyNo);

            exceptionCommisions.ForEach(ec =>
            {
                ec.ClientId = clientId;
                ec.ClientPolicyId = clientPolicies.Id;
                _commissionRepository.UpdateExceptionCommisionsClient(ec);
            });
            
            return true;
        }
    }
}