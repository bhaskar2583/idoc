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
            _hanysContext = new IBSDbContext();
        }

        public bool Add(Policies policy)
        {
            _hanysContext.Policies.Add(policy);
            _hanysContext.SaveChanges();
            return true;
        }

        public bool Delete(int id)
        {
            var policy = new Policies()
            {
                Id = id
            };

            _hanysContext.Policies.Attach(policy);
            _hanysContext.Policies.Remove(policy);
            _hanysContext.SaveChanges();
            return true;
        }

        public IQueryable<Policies> GetAll()
        {
            return _hanysContext.Policies;
        }

        public Policies GetById(int id)
        {
            var entity = GetAll().FirstOrDefault(c => c.Id == id);

            return entity;
        }

        public bool Update(Policies policy)
        {
            var data = _hanysContext.Policies.FirstOrDefault(c => c.Id == policy.Id);
            if (data != null)
            {
                data.IsActive = policy.IsActive;
                data.CarId = policy.CarId;
                data.PolicyNumber = policy.PolicyNumber;
                data.PolicyType = policy.PolicyType;
                data.EffectiveDate = policy.EffectiveDate;
                data.EndDate = policy.EndDate;
                data.IsGroupInsurance = policy.IsGroupInsurance;
                data.EndDate = policy.EndDate;
                data.AddUser = policy.AddUser;
                data.AddDate = policy.AddDate;
                data.RevDate = policy.RevDate;
                data.RevUser = policy.RevUser;
                _hanysContext.SaveChanges();
            }
            return true;
        }
    }
}
