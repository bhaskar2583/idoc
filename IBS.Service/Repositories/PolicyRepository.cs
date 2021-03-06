﻿using IBS.Core.Entities;
using IBS.Service.DataBaseContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBS.Service.Repositories
{
    public class PolicyRepository : IPolicyRepository
    {
        private readonly IBSDbContext _hanysContext;
        public PolicyRepository()
        {
            _hanysContext = new IBSDbContext();
        }

        public bool Add(Policie policy)
        {
            _hanysContext.Policies.Add(policy);
            _hanysContext.SaveChanges();
            return true;
        }

        public bool Delete(int id)
        {
            var policy = new Policie()
            {
                Id = id
            };

            _hanysContext.Policies.Attach(policy);
            _hanysContext.Policies.Remove(policy);
            _hanysContext.SaveChanges();
            return true;
        }

        public IQueryable<Policie> GetAll()
        {
            return _hanysContext.Policies;
        }

        public Policie GetById(int id)
        {
            var entity = GetAll().FirstOrDefault(c => c.Id == id);

            return entity;
        }

        public Policie GetPolicyByNoCarriageCoverage(string policyNo, int carrierId, int coverageId, int product)
        {
            var entity = GetAll().FirstOrDefault(c => c.PolicyNumber == policyNo
            && c.CarId == carrierId
            && c.CoverageId == coverageId
            && c.ProductId == product);

            return entity;
        }

        public bool Update(Policie policy)
        {
            var data = _hanysContext.Policies.FirstOrDefault(c => c.Id == policy.Id);
            if (data != null)
            {
                data.IsActive = policy.IsActive;
                data.CarId = policy.CarId;
                data.CoverageId = policy.CoverageId;
                data.ProductId = policy.ProductId;
                data.PolicyNumber = policy.PolicyNumber;
                data.EffectiveDate = policy.EffectiveDate;
                data.EndDate = policy?.EndDate;
                data.IsGroupInsurance = policy.IsGroupInsurance;
                data.RevDate = policy.RevDate;
                data.RevUser = policy.RevUser;
                _hanysContext.SaveChanges();
            }
            return true;
        }
    }
}