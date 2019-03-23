using IBS.Core.Entities;
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
            _hanysContext = SingletonForEF.Instance;
        }

        public bool Add(Policy policy)
        {
            _hanysContext.pps.Add(policy);
            _hanysContext.SaveChanges();
            return true;
        }

        public bool Delete(int id)
        {
            var policy = new Policy()
            {
                Id = id
            };

            _hanysContext.pps.Attach(policy);
            _hanysContext.pps.Remove(policy);
            _hanysContext.SaveChanges();
            return true;
        }

        public IQueryable<Policy> GetAll()
        {
            return _hanysContext.pps;
        }

        public Policy GetById(int id)
        {
            var entity = GetAll().FirstOrDefault(c => c.Id == id);

            return entity;
        }

        public bool Update(Policy policy)
        {
            var data = _hanysContext.pps.FirstOrDefault(c => c.Id == policy.Id);
            if (data != null)
            {
                data.Name = policy.Name;
                data.IsActive = policy.IsActive;
                data.CarId = policy.CarId;
                data.EffectiveDate = policy.EffectiveDate;
                data.EndDate = policy.EndDate;
                data.IsGroupInsurence = policy.IsGroupInsurence;
                data.PolicyType = policy.PolicyType;
                data.RevDate = policy.RevDate;
                data.RevUser = policy.RevUser;
                _hanysContext.SaveChanges();
            }
            return true;
        }
    }
}
