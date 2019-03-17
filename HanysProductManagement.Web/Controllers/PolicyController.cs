using HanysProductManagement.Service.Models;
using HanysProductManagement.Service.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HanysProductManagement.Web.Controllers
{
    public class PolicyController : Controller
    {
        private readonly IPolicyService _policyService;
        public PolicyController(IPolicyService policyService)
        {
            _policyService = policyService;
        }
        // GET: Policies
        public ActionResult Index()
        {
            var policies = _policyService.GetAllPolicies();
            return View(policies);
        }
        // GET: Policy/AddPolicy 
        public ActionResult AddPolicy()
        {
            return View();
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

                return View();
            }
            catch
            {
                return View();
            }
        }

        // GET: Policy/EditPolicyDetails/1   
        public ActionResult EditPolicyDetails(int id)
        {
            var policy = _policyService.GetById(id);
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