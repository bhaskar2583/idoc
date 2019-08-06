using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBS.Core.Entities;
using IBS.Core.Models;
using IBS.Service.DataBaseContext;
using IBS.Service.Utils;

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

        public List<Commision> GetSavedCommisions()
        {
          return  _hanysContext.Commisions.ToList();
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
                data.CommisionString = commission.CommisionString;
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

        public List<InvalidCommission> GetExceptionCommissionsForCarrier(int carrierId)
        {
            if(carrierId==0)
                return _hanysContext.InvalidCommissions.Where(ec => !ec.IsDumped).ToList();

            return _hanysContext.InvalidCommissions.Where(ec => !ec.IsDumped &&
             ec.CarrierId == carrierId).ToList();
        }

        public List<int> GetExceptionCommissionsCariers()
        {
            try
            {
                var data = _hanysContext.InvalidCommissions.ToList();
                return _hanysContext.InvalidCommissions.Where(ec => !ec.IsDumped).Select(c => c.CarrierId).Distinct().ToList();
            }
            catch (Exception ex)
            {
                return new List<int>();
            }
            
        }
        private string GetDateFormat(DateTime? date)
        {
            if (date == null)
                return string.Empty;

            DateTime dt = Convert.ToDateTime(date);
            return dt.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
        }
        public List<SelectListCommon> GetExceptionCarrierStatementDates(int carrierId)
        {
            try
            {
                var dates = new List<SelectListCommon>();

             var commossions= _hanysContext.InvalidCommissions.Where(ec => !ec.IsDumped &&
             ec.CarrierId == carrierId).ToList().Select(d => new SelectListCommon()
             {
                 Id = 0,
                 Name = GetDateFormat(d.StatementDate)
             }).Distinct().ToList();

                commossions.ForEach(c =>
                {
                    if (dates.FirstOrDefault(d=>d.Name==c.Name) == null)
                    {
                        dates.Add(c);
                    }
                });
                return dates;
            }
            catch (Exception ex)
            {
                return new List<SelectListCommon>();
            }
            
        }

        public bool UpdateExceptionCommisions(InvalidCommission commission)
        {
            var eCommission = _hanysContext.InvalidCommissions.FirstOrDefault(ec => ec.Id == commission.Id);
            if (eCommission != null)
            {
                eCommission.IsDumped = true;
                _hanysContext.SaveChanges();
            }

            return true;
        }

        public InvalidCommission GetExceptionCommissionsById(int Id)
        {
            return _hanysContext.InvalidCommissions.FirstOrDefault(ec => ec.Id ==Id);
        }

        public bool UpdateExceptionCommisionsClient(InvalidCommission commission)
        {
            var eCommission = _hanysContext.InvalidCommissions.FirstOrDefault(ec => ec.Id == commission.Id);
            if (eCommission != null)
            {
                eCommission.ClientId = commission.ClientId;
                eCommission.ClientPolicyId = commission.ClientPolicyId;
                _hanysContext.SaveChanges();
            }

            return true;
        }
    }
}