using IBS.Core.Enums;
using IBS.Core.Models;
using IBS.Service.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IBS.Controllers
{
    public class CarrierController : Controller
    {
        private readonly ICarrierService _carrierService;
        public CarrierController(ICarrierService carrierService)
        {
            _carrierService = carrierService;
        }
        // GET: Carrier
        public ActionResult Index(string carrierSearchkey,string statusSearchkey="None")
        {
            var carriers = _carrierService.GetAllCarriers();

            Enum.TryParse(statusSearchkey, out CarrierStatusEnum myStatus);
            carriers = _carrierService.ApplyFilterForIndex(carrierSearchkey, myStatus, carriers);

            return View(carriers);
        }
        // GET: Carrier/AddCarrier  
        public ActionResult AddCarrier()
        {
            return View();
        }

        // POST: Carrier/AddCarrier    
        [HttpPost]
        public ActionResult AddCarrier(CarrierModel carrier)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = _carrierService.AddCarrier(carrier);

                    if (result)
                    {
                        ViewBag.Message = "Career details added successfully";
                    }
                }

                return View();
            }
            catch(Exception ex)
            {
                return View();
            }
        }

        // GET: Carrier/EditCareerDetails/1   
        public ActionResult EditCarrierDetails(int id)
        {
            var carrier = _carrierService.GetById(id);
            return View(carrier);
        }

        // POST: Carrier/EditCrrierDetails/1   
        [HttpPost]

        public ActionResult EditCarrierDetails(int id, CarrierModel obj)
        {
            try
            {
                _carrierService.ModifyCarrier(obj);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Carrier/DeleteCareer/1    
        public ActionResult DeleteCarrier(int id)
        {
            try
            {
                var result = _carrierService.DeleteCarrier(id);
                if (result)
                {
                    ViewBag.AlertMsg = "Carrier details deleted successfully";
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}