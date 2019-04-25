using IBS.Core.Models;
using IBS.Service.Services;
using IBS.Service.Utils;
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
        public ActionResult Index(int? carrierId,bool? isSaved)
        {
            ViewBag.Carriers = _carrierService.GetAllCarriers();
            var commisions = new List<CommisionModel>();
            if(carrierId!=null && carrierId > 0)
            {
                ViewBag.Status = CommonUtil.GetStatus();
                commisions = _commisionService.GetCarrierPoliciesById(Convert.ToInt32(carrierId));

                
            }
            ViewBag.PersistMessage = isSaved!=null && isSaved==true ? "Commission added successfully" : "";
            return View(commisions);
        }

        [HttpPost]
        // post: Commision
        public ActionResult Index(List<CommisionModel> commisions)
        {
            commisions.ForEach(c =>
            {
                var policy = _commisionService.GetPolicyByNoCarriageCoverage(c.PolicyNumber, c.CarrierId, c.CoverageId);
                var clientPolicy = _commisionService.GetClientPoliciesByPolicyId(policy.Id);
                c.ClientPolicyId = clientPolicy.Id;
            });
            _commisionService.SaveCommisions(commisions);
            //return RedirectToAction("Index", new { carrierId = commisions[0].CarrierId, isSaved=true });
            return Json(_commisionService.GetCarrierPoliciesById(1), JsonRequestBehavior.AllowGet);
        }
    }
}