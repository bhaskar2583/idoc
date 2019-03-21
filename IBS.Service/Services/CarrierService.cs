using IBS.Core.Entities;
using IBS.Core.Models;
using IBS.Service.Repositories;
using IBS.Service.Utils;
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
                Address=carrier.Address,
                Email=carrier.Email,
                Phone=carrier.Phone,
                IsActive=true,
                AddUser= LoginUserDetails.GetWindowsLoginUserName(),
                AddDate= DateUtil.GetCurrentDate()
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
                    Name = entity.Name,
                    Address=entity.Address,
                    Email=entity.Email,
                    AddDate=entity.AddDate,
                    AddUser=entity.AddUser,
                    Phone=entity.Phone,
                    RevDate=entity.RevDate,
                    RevUser=entity.RevUser
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

            carriersData.ForEach(entity =>
            {
                var carrier = new CarrierModel()
                {
                    Id = entity.Id,
                    IsActive = (bool)entity.IsActive,
                    Name = entity.Name,
                    Address = entity.Address,
                    Email = entity.Email,
                    AddDate = entity.AddDate,
                    AddUser = entity.AddUser,
                    Phone = entity.Phone,
                    RevDate = entity.RevDate,
                    RevUser = entity.RevUser
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
                IsActive = (bool)carrier.IsActive,
                Name = carrier.Name,
                Address = carrier.Address,
                Email = carrier.Email,
                AddDate = carrier.AddDate,
                AddUser = carrier.AddUser,
                Phone = carrier.Phone,
                RevDate = DateUtil.GetCurrentDate(),
                RevUser = LoginUserDetails.GetWindowsLoginUserName()
            };

            return _carrierRepository.Update(entity);
        }
    }
}
