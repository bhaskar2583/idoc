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
    }
}
