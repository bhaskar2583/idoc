using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HanysProductManagement.Service.Context;
using HanysProductManagement.Service.Entites;

namespace HanysProductManagement.Service.Repositories
{
    public class CarrierRepository : ICarrierRepository
    {
        private readonly HanysContext _hanysContext;
        public CarrierRepository()
        {
            _hanysContext = new HanysContext();
        }

        public bool Add(Carriers carrier)
        {
            _hanysContext.Carriers.Add(carrier);
            _hanysContext.SaveChanges();
            return true;
        }

        public bool Delete(int id)
        {
            var carrier = new Carriers()
            {
                Id = id
            };

            _hanysContext.Carriers.Attach(carrier);
            _hanysContext.Carriers.Remove(carrier);
            _hanysContext.SaveChanges();
            return true;
        }

        public IQueryable<Carriers> GetAll()
        {
           return _hanysContext.Carriers;
        }

        public Carriers GetById(int id)
        {
            var entity = GetAll().FirstOrDefault(c => c.Id == id);

            return entity;
        }

        public bool Update(Carriers carrier)
        {
            var data = _hanysContext.Carriers.FirstOrDefault(c => c.Id == carrier.Id);
            if (data != null){
                data.Name = carrier.Name;
                data.IsActive = carrier.IsActive;
                _hanysContext.SaveChanges();
            }
            return true;
        }
    }
}
