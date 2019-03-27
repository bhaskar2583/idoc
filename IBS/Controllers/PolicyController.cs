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
    public class PolicyController : System.Web.Mvc.Controller
    {
        private readonly IPolicyService _policyService;
        private readonly ICarrierService _carrierService;
        public PolicyController(IPolicyService policyService, ICarrierService carrierService)
        {
            _policyService = policyService;
            _carrierService = carrierService;
        }
        // GET: Policies
        public ActionResult Index(string searchkey, string statusSearchkey = "Active")
        {
            var policies = _policyService.GetAllPolicies();
            Enum.TryParse(statusSearchkey, out CarrierStatusEnum myStatus);
            policies = _policyService.ApplyFilterForIndex(searchkey, myStatus, policies);
            return View(policies);
        }
        // GET: Policy/AddPolicy 
        public ActionResult AddPolicy()
        {
            var policy = new PolicyModel();
            _policyService.MapCarriers(policy);
            return View(policy);
        }

        // POST: Policy/AddPolicy   
        [HttpPost]
        public ActionResult AddPolicy(PolicyModel policy)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = _policyService.AddPolicy(policy);

                    if (result)
                    {
                        ViewBag.Message = "Policy details added successfully";
                    }
                }
                _policyService.MapCarriers(policy);
                return View(policy);
            }
            catch(Exception ex)
            {
                return View();
            }
        }

        // GET: Policy/EditPolicyDetails/1   
        public ActionResult EditPolicyDetails(int id)
        {
            var policy = _policyService.GetById(id);
            _policyService.MapCarriers(policy);
            return View(policy);
        }

        // POST: Policy/EditPolicyDetails/1   
        [HttpPost]

        public ActionResult EditPolicyDetails(int id, PolicyModel policy)
        {
            try
            {
                _policyService.ModifyPolicy(policy);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Policy/DeletePolicy/1    
        public ActionResult DeletePolicy(int id)
        {
            try
            {
                var result = _policyService.DeletePolicy(id);
                if (result)
                {
                    ViewBag.AlertMsg = "Policy details deleted successfully";
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