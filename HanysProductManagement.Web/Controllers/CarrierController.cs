using HanysProductManagement.Service.Models;
using HanysProductManagement.Service.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HanysProductManagement.Web.Controllers
{
    public class CarrierController : Controller
    {
        private readonly ICarrierService _carrierService;
        public CarrierController(ICarrierService carrierService)
        {
            _carrierService = carrierService;
        }
        // GET: Carrier
        public ActionResult Index()
        {
            var carriers = _carrierService.GetAllCarriers();
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
            catch
            {
                return View();
            }
        }

        // GET: Carrier/EditCareerDetails/1   
        public ActionResult EditCareerDetails(int id)
        {
            var carrier = _carrierService.GetById(id);
            return View(carrier);
        }

        // POST: Carrier/EditCareerDetails/1   
        [HttpPost]

        public ActionResult EditCareerDetails(int id, CarrierModel obj)
        {
            try
            {
                //EmpRepository EmpRepo = new EmpRepository();
                //EmpRepo.UpdateEmployee(obj);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Carrier/DeleteCareer/1    
        public ActionResult DeleteCareer(int id)
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