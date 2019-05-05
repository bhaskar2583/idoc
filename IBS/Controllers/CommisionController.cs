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
            var commisions = new List<CommisionModel>();
            try
            {
                ViewBag.Carriers = _carrierService.GetAllCarriers();
                
                if (carrierId != null && carrierId > 0)
                {
                    ViewBag.Status = CommonUtil.GetStatus();
                    commisions = _commisionService.GetCarrierPoliciesById(Convert.ToInt32(carrierId));
                }
                ViewBag.PersistMessage = isSaved != null && isSaved == true ? "Commission added successfully" : "";
                return View(commisions);
            }
            catch (Exception ex)
            {
                return View(commisions);
            }
           
        }

        [HttpPost]
        // post: Commision
        public ActionResult Index(List<CommisionModel> commisions)
        {
            var savedCommisions = commisions.Where(cpm => cpm.CoverageId != null && cpm.CoverageId > 0
             && cpm.ProductId != null && cpm.ProductId > 0 && cpm.StatementDate!=null && cpm.CommisionValue!=null && cpm.CommisionValue>0).ToList();
            savedCommisions.ForEach(c =>
            {
                var policy = _commisionService.GetPolicyByNoCarriageCoverage(c.PolicyNumber, c.CarrierId, c.CoverageId,c.ProductId);
                var clientPolicy = _commisionService.GetClientPoliciesByPolicyId(policy.Id);
                c.ClientPolicyId = clientPolicy.Id;
            });
            _commisionService.SaveCommisions(savedCommisions);
            //return RedirectToAction("Index", new { carrierId = commisions[0].CarrierId, isSaved=true });
            return Json(_commisionService.GetCarrierPoliciesById(1), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        // post: Commision
        public ActionResult ProductsOfPolicy(string client,string policyNo,string coverage)
        {
            var products = _commisionService.GetProductsOfPolicy(client, policyNo, coverage);
            products = products.Distinct().ToList();
            return Json(products, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditCommissions(int? carrierId, string smd,string pId)
        {
            var commisions = new List<CommisionModel>();
            try
            {
                ViewBag.Carriers = _carrierService.GetAllCarriers();

                if (carrierId != null && carrierId > 0)
                {
                    ViewBag.Status = CommonUtil.GetStatus();
                    commisions = _commisionService.GetAllSavedCommissionsForCarrier(Convert.ToInt32(carrierId));
                }
                //ViewBag.PersistMessage = isSaved != null && isSaved == true ? "Commission added successfully" : "";
                commisions = commisions.Where(c => c.CoverageId == carrierId).ToList();
                if (!string.IsNullOrEmpty(smd) && commisions!=null && commisions.Count>0)
                {
                    commisions = commisions.Where(c => c.StatementDateAsString == smd).ToList();
                }
                if (!string.IsNullOrEmpty(pId) && commisions != null && commisions.Count > 0)
                {
                    commisions = commisions.Where(c => c.PaymentId == pId).ToList();
                }
                return View(commisions);
            }
            catch (Exception ex)
            {
                return View(commisions);
            }

        }
    }
}