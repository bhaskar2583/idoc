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

        public bool Add(Policy policy)
        {
            _hanysContext.Policies.Add(policy);
            _hanysContext.SaveChanges();
            return true;
        }

        public bool Delete(int id)
        {
            var policy = new Policy()
            {
                Id = id
            };

            _hanysContext.Policies.Attach(policy);
            _hanysContext.Policies.Remove(policy);
            _hanysContext.SaveChanges();
            return true;
        }

        public IQueryable<Policy> GetAll()
        {
            return _hanysContext.Policies;
        }

        public Policy GetById(int id)
        {
            var entity = GetAll().FirstOrDefault(c => c.Id == id);

            return entity;
        }

        public bool Update(Policy policy)
        {
            var data = _hanysContext.Policies.FirstOrDefault(c => c.Id == policy.Id);
            if (data != null)
            {
                data.Name = policy.Name;
                data.Active = policy.Active;
                _hanysContext.SaveChanges();
            }
            return true;
        }
    }
}