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
            var entity = new Policy()
            {
                Name = policy.Name,
                IsActive = policy.IsActive,
                AddUser = LoginUserDetails.GetWindowsLoginUserName(),
                AddDate = DateUtil.GetCurrentDate(),
                CarId=policy.CarId,
                EffectiveDate=policy.EffectiveDate,
                EndDate=policy.EndDate,
                IsGroupInsurence=policy.IsGroupInsurence,
                PolicyType=policy.PolicyType
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

            var policiesData = _policyRepository.GetAll();

            if (policiesData == null)
            {
                return policies;
            }

            policiesData.ToList().ForEach(entity =>
            {
                var policy = new PolicyModel()
                {
                    Name = entity.Name,
                    IsActive = entity.IsActive,
                    AddUser = entity.AddUser,
                    AddDate = entity.AddDate,
                    CarId = entity.CarId,
                    EffectiveDate = entity.EffectiveDate,
                    EndDate = entity.EndDate,
                    IsGroupInsurence = entity.IsGroupInsurence,
                    PolicyType = entity.PolicyType,
                    Id = entity.Id,
                    RevDate = entity.RevDate,
                    RevUser = entity.RevUser
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
                    Name = entity.Name,
                    IsActive = entity.IsActive,
                    AddUser = entity.AddUser,
                    AddDate = entity.AddDate,
                    CarId = entity.CarId,
                    EffectiveDate = entity.EffectiveDate,
                    EndDate = entity.EndDate,
                    IsGroupInsurence = entity.IsGroupInsurence,
                    PolicyType = entity.PolicyType,
                    Id=entity.Id,
                    RevDate=entity.RevDate,
                    RevUser=entity.RevUser
                };
            }

            return null;
        }

        public bool ModifyPolicy(PolicyModel policy)
        {
            var entity = new Policy()
            {
                Name = policy.Name,
                IsActive = policy.IsActive,
                CarId = policy.CarId,
                EffectiveDate = policy.EffectiveDate,
                EndDate = policy.EndDate,
                IsGroupInsurence = policy.IsGroupInsurence,
                PolicyType = policy.PolicyType,
                RevDate = DateUtil.GetCurrentDate(),
                RevUser = LoginUserDetails.GetWindowsLoginUserName()
            };

            return _policyRepository.Update(entity);
        }
    }
}
