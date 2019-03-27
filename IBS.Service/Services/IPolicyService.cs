﻿using IBS.Core.Enums;
using IBS.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBS.Service.Services
{
    public interface IPolicyService
    {
        IList<PolicyModel> GetAllPolicies();
        PolicyModel GetById(int Id);
        bool AddPolicy(PolicyModel policy);
        bool ModifyPolicy(PolicyModel policy);
        bool DeletePolicy(int policyId);

        void MapCarriers(PolicyModel policy);
        IList<PolicyModel> ApplyFilterForIndex(string policyNumber, CarrierStatusEnum searchStatus, IList<PolicyModel> source);
    }
}
