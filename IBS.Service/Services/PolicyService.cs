using IBS.Core.Entities;
using IBS.Core.Models;
using IBS.Service.Repositories;
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
                Active = policy.IsActive
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
                    IsActive = (bool)c.Active,
                    Name = c.Name
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
                    IsActive = (bool)entity.Active,
                    Name = entity.Name
                };
            }

            return null;
        }

        public bool ModifyPolicy(PolicyModel policy)
        {
            var entity = new Policy()
            {
                Id = policy.Id,
                Name = policy.Name,
                Active = policy.IsActive
            };

            return _policyRepository.Update(entity);
        }
    }
}
