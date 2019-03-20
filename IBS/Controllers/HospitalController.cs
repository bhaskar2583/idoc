using IBS.Core.Models;
using IBS.Service.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IBS.Controllers
{
    public class HospitalController : Controller
    {
        private readonly IHospitalService _hospitalService;
        public HospitalController(IHospitalService hospitalService)
        {
            _hospitalService = hospitalService;
        }
        // GET: Carrier
        public ActionResult Index()
        {
            var hospitals = _hospitalService.GetAllHospitals();
            return View(hospitals);
        }
        // GET: Hospital/AddHospital  
        public ActionResult AddHospital()
        {
            return View();
        }

        // POST: Hospital/AddHospital    
        [HttpPost]
        public ActionResult AddHospital(HospitalModel hospital)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = _hospitalService.AddHospital(hospital);

                    if (result)
                    {
                        ViewBag.Message = "Hospital details added successfully";
                    }
                }

                return View();
            }
            catch
            {
                return View();
            }
        }

        // GET: Hospital/EditHospitalDetails/1   
        public ActionResult EditHospitalDetails(int id)
        {
            var hospital = _hospitalService.GetById(id);
            return View(hospital);
        }

        // POST: Hospital/EditHospitalDetails/1   
        [HttpPost]

        public ActionResult EditHospitalDetails(int id, HospitalModel obj)
        {
            try
            {
                _hospitalService.ModifyHospital(obj);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Hospital/DeleteHospital/1    
        public ActionResult DeleteHospital(int id)
        {
            try
            {
                var result = _hospitalService.DeleteHospital(id);
                if (result)
                {
                    ViewBag.AlertMsg = "Hospital details deleted successfully";
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