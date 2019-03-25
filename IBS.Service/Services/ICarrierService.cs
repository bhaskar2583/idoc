using IBS.Core.Enums;
using IBS.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBS.Service.Services
{
    public interface ICarrierService
    {
        IList<CarrierModel> GetAllCarriers();
        CarrierModel GetById(int Id);
        bool AddCarrier(CarrierModel carrier);
        bool ModifyCarrier(CarrierModel carrier);
        bool DeleteCarrier(int carrierId);
        IList<CarrierModel> ApplyFilterForIndex(string carrierName, CarrierStatusEnum searchStatus, IList<CarrierModel> source);
        //IList<CarrierModel> ApplyFilterForIndex(object carrierSearchkey, CarrierStatusEnum myStatus, IList<CarrierModel> carriers);
    }
}
