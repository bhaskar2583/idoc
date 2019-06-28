using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBS.Core.Entities;
using IBS.Core.Models;
using IBS.Service.DataBaseContext;

namespace IBS.Service.Repositories
{
    public class CommisionRepository : ICommisionRepository
    {
        private readonly IBSDbContext _hanysContext;
        public CommisionRepository()
        {
            _hanysContext = SingletonForEF.Instance;
        }
        public bool Add(Commision commission)
        {
            _hanysContext.Commisions.Add(commission);
            _hanysContext.SaveChanges();
            return true;
        }

        public Commision GetByClientPolicyId(int clientPolicyId)
        {
            return _hanysContext.Commisions.FirstOrDefault(cov => cov.ClientPolicyId == clientPolicyId);
        }
        public List<Commision> GetSavedCommisionsForCarrier(int carrierId)
        {
            return _hanysContext.Commisions.ToList();//.Where(cov => cov.CarrierId == carrierId).ToList();
        }
        public List<Commision> GetByAllClientPolicyId(int clientPolicyId)
        {
            return _hanysContext.Commisions.Where(cov => cov.ClientPolicyId == clientPolicyId).ToList();
        }
        public bool Update(CommisionModel commission)
        {
            var data = _hanysContext.Commisions.FirstOrDefault(c => c.ClientPolicyId == commission.ClientPolicyId);
            if (data != null)
            {
                data.CommisionValue = commission.CommisionValue;
                //data.PaymentId = commission.PaymentId;
                //data.StatementDate = commission.StatementDate;
                data.AppliedDate = commission.AppliedDate;
                data.RevDate = commission.RevDate;
                data.RevUser = commission.RevUser;
                //data.Status = commission.Status;
                _hanysContext.SaveChanges();
            }
            return true;
        }

        public bool UpdateCommissionReconsilation(CommisionModel commission)
        {
            try
            {
                var data = _hanysContext.Commisions.Where(c => c.Id == commission.Id).ToList();
                if (data != null)
                {
                    data.ForEach(d =>
                    {
                        d.CommisionValue = commission.CommisionValue;
                        d.ReconcilationDate = commission.AppliedDate;
                        d.ReconcilationStatus = commission.ReconsilationStatus;
                        d.RevDate = commission.RevDate;
                        d.RevUser = commission.RevUser;
                        _hanysContext.SaveChanges();
                    });

                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }
    }
}