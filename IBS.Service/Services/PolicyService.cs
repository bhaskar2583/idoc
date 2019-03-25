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
    public class PolicyService : IPolicyService
    {
        private readonly IPolicyRepository _policyRepository;
        public PolicyService(IPolicyRepository policyRepository)
        {
            _policyRepository = policyRepository;
        }

        public bool AddPolicy(PolicyModel policy)
        {
            var entity = new Policie()
            {
                PolicyNumber = policy.PolicyNumber,
                PolicyType = policy.PolicyType,
                CarId = policy.CarId,
                EffectiveDate = policy.EffectiveDate,
                EndDate = policy.EndDate,
                IsGroupInsurance = policy.IsGroupInsurance,
                IsActive = policy.IsActive,
                AddUser = LoginUserDetails.GetWindowsLoginUserName(),
                AddDate = DateUtil.GetCurrentDate()
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

            policiesData.ForEach(c =>
            {
                var policy = new PolicyModel()
                {
                    Id = c.Id,
                    IsActive = (bool)c.IsActive,
                    PolicyNumber = c.PolicyNumber,
                    PolicyType = c.PolicyType,
                    CarId = c.CarId,
                    EffectiveDate = c.EffectiveDate,
                    EndDate = c.EndDate,
                    IsGroupInsurance = (bool)c.IsGroupInsurance,
                    AddUser = c.AddUser,
                    AddDate = c.AddDate,
                    RevUser = c.RevUser,
                    RevDate = c.RevDate
                };

                policies.Add(policy);
            });

            return policies;
        }

        public PolicyModel GetById(int Id)
        {
            var entity = _policyRepository.GetById(Id);

            if (entity != null)
            {
                return new PolicyModel()
                {
                    Id = entity.Id,
                    IsActive = (bool)entity.IsActive,
                    PolicyNumber = entity.PolicyNumber,
                    PolicyType = entity.PolicyType,
                    CarId = entity.CarId,
                    EffectiveDate = entity.EffectiveDate,
                    EndDate = entity.EndDate,
                    IsGroupInsurance = (bool)entity.IsGroupInsurance,
                    AddUser = entity.AddUser,
                    AddDate = entity.AddDate,
                    RevUser = entity.RevUser,
                    RevDate = entity.RevDate
                };
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
                EffectiveDate = policy.EffectiveDate,
                EndDate = policy.EndDate,
                IsGroupInsurance = policy.IsGroupInsurance,
                AddUser = policy.AddUser,
                AddDate = policy.AddDate,
                RevDate = DateUtil.GetCurrentDate(),
                RevUser = LoginUserDetails.GetWindowsLoginUserName()
            };

            return _policyRepository.Update(entity);
        }
    }
}
