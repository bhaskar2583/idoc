using IBS.Core.Models;
using IBS.Service.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IBS.Controllers
{
    public class CommisionController : Controller
    {
        private readonly ICarrierService _carrierService;
        private readonly ICommisionService _commisionService;
        public CommisionController(ICarrierService carrierService, ICommisionService commisionService)
        {
            _carrierService = carrierService;
            _commisionService = commisionService;
        }
        // GET: Commision
        public ActionResult Index(int? carrierId)
        {
            ViewBag.Carriers = _carrierService.GetAllCarriers();
            var commisions = new List<CommisionModel>();
            if(carrierId!=null && carrierId > 0)
            {
                commisions = _commisionService.GetCarrierPoliciesById(Convert.ToInt32(carrierId));
            }
            return View(commisions);
        }

        [HttpPost]
        // post: Commision
        public ActionResult Index(List<CommisionModel> commisions)
        {
            _commisionService.SaveCommisions(commisions);
            return RedirectToAction("Index", new { carrierId = commisions[0].CarrierId });
        }
    }
}