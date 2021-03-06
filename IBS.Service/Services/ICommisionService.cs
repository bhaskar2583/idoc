﻿using IBS.Core.Entities;
using IBS.Core.Models;
using IBS.Service.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBS.Service.Services
{
    public interface ICommisionService
    {
        List<Commision> GetSavedCommisions();
        ClientPolicie GetAllClientPoliciesByIndexId(int Id);
        List<CommisionModel> GetCarrierPoliciesById(int carrierId);
        List<CommisionModel> GetAllSavedCommissionsForCarrier(int carrierId);
        bool SaveCommisions(List<CommisionModel> commissions, bool save);
        bool UpdateCommisions(List<CommisionModel> commissions);
        Policie GetPolicyByNoCarriageCoverage(string policyNo, int carrierId, int coverageId, int product);
        Policie GetPolicyByNoCarriageCoverage(string policyNo, int carrierId, int coverageId);
        ClientPolicie GetClientPoliciesByPolicyId(int policyId);
        List<Product> GetProductsOfPolicy(string client, string policy, string coverage);
        List<SelectListCommon> GetCarrierStatementDates(string carrierId);
        List<SelectListCommon> GetCarrierStatementDatePayments(string carrierId, string statementDate);
        IList<CorporateProduct> GetAllCorporateProducts();
        List<ExceptionCommissionModel> GetAllExceptionCommissionsForCarrier(int? carrierId);
        IList<Carrier> GetExceptionCommissionsCariers();
        List<SelectListCommon> GetExceptionCarrierStatementDates(int? carrierId);
        bool UpdateExceptionCommisions(List<ExceptionCommissionModel> commissions);
        bool UpdateExceptionCommisionsClient(int Id, int clientId, int policyId,string policyNo);
    }
}