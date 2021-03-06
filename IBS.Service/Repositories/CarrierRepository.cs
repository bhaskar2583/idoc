﻿using IBS.Core.Entities;
using IBS.Service.DataBaseContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBS.Service.Repositories
{
    public class CarrierRepository : ICarrierRepository
    {
        private readonly IBSDbContext _hanysContext;
        public CarrierRepository()
        {
            _hanysContext = SingletonForEF.Instance;
        }

        public bool Add(Carrier carrier)
        {
            _hanysContext.Carriers.Add(carrier);
            _hanysContext.SaveChanges();
            return true;
        }

        public bool Delete(int id)
        {
            var carrier = new Carrier()
            {
                Id = id
            };

            _hanysContext.Carriers.Attach(carrier);
            _hanysContext.Carriers.Remove(carrier);
            _hanysContext.SaveChanges();
            return true;
        }

        public IQueryable<Carrier> GetAll()
        {
            return _hanysContext.Carriers;
        }

        public Carrier GetById(int id)
        {
            var entity = GetAll().FirstOrDefault(c => c.Id == id);

            return entity;
        }

        public bool Update(Carrier carrier)
        {
            var data = _hanysContext.Carriers.FirstOrDefault(c => c.Id == carrier.Id);
            if (data != null)
            {
                data.Name = carrier.Name;
                data.Email = carrier.Email;
                data.Phone = carrier.Phone;
                data.AddressLine1 = carrier.AddressLine1;
                data.AddressLine2 = carrier.AddressLine2;
                data.City = carrier.City;
                data.State = carrier.State;
                data.ZipCode = carrier.ZipCode;
                data.RevUser = carrier.RevUser;
                data.RevDate = carrier.RevDate;
                data.IsActive = carrier.IsActive;
                _hanysContext.SaveChanges();
            }
            return true;
        }
    }
}