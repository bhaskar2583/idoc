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
    public class CarrierService : ICarrierService
    {
        private readonly ICarrierRepository _carrierRepository;
        public CarrierService(ICarrierRepository carrierRepository)
        {
            _carrierRepository = carrierRepository;
        }
        public bool AddCarrier(CarrierModel carrier)
        {
            var entity = new Carrier()
            {
                Name = carrier.Name,
                IsActive = carrier.IsActive
            };

            return _carrierRepository.Add(entity);
        }

        public bool DeleteCarrier(int carrierId)
        {
            return _carrierRepository.Delete(carrierId);
        }

        public CarrierModel GetById(int Id)
        {
            var entity = _carrierRepository.GetById(Id);

            if (entity != null)
            {
                return new CarrierModel()
                {
                    Id = entity.Id,
                    IsActive = (bool)entity.IsActive,
                    Name = entity.Name
                };
            }

            return null;

        }

        public IList<CarrierModel> GetAllCarriers()
        {
            var carriers = new List<CarrierModel>();

            var carriersData = _carrierRepository.GetAll().ToList();

            if (carriersData == null)
            {
                return carriers;
            }

            carriersData.ForEach(c =>
            {
                var carrier = new CarrierModel()
                {
                    Id = c.Id,
                    IsActive = (bool)c.IsActive,
                    Name = c.Name
                };

                carriers.Add(carrier);
            });

            return carriers;
        }

        public bool ModifyCarrier(CarrierModel carrier)
        {
            var entity = new Carrier()
            {
                Id = carrier.Id,
                Name = carrier.Name,
                IsActive = carrier.IsActive
            };

            return _carrierRepository.Update(entity);
        }
    }
}
