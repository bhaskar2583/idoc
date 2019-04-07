using IBS.Core.Enums;
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
    public class PolicyController : System.Web.Mvc.Controller
    {
        private readonly IPolicyService _policyService;
        private readonly ICarrierService _carrierService;
        private readonly ICommonService _commonService;
        private readonly IClientService _clinetService;
        public PolicyController(IPolicyService policyService, ICarrierService carrierService,
            ICommonService commonService, IClientService clinetService)
        {
            _policyService = policyService;
            _carrierService = carrierService;
            _commonService = commonService;
            _clinetService = clinetService;
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
            _policyService.MapCoverages(policy);
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
                _policyService.MapCoverages(policy);
                _policyService.MapProductsOfCoverage(policy, policy.CoverageId);
                return View(policy);
            }
            catch(Exception ex)
            {
                _policyService.MapCarriers(policy);
                _policyService.MapCoverages(policy);
                _policyService.MapProductsOfCoverage(policy, policy.CoverageId);
                ViewBag.Message = "Error while adding policy details";
                return View(policy);
            }
        }

        // GET: Policy/EditPolicyDetails/1   
        public ActionResult EditPolicyDetails(int id)
        {
            var policy = _policyService.GetById(id);
            _policyService.MapCarriers(policy);
            _policyService.MapCoverages(policy);
            _policyService.MapProductsOfCoverage(policy,policy.SelectedCoverage.Id);
            return View(policy);
        }

        // GET: Policy/EditPolicyDetails/1   
        public ActionResult ViewPolicyDetails(int id)
        {
            var policy = _policyService.GetById(id);
            _policyService.MapProductsOfCoverage(policy, policy.SelectedCoverage.Id);
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
                _policyService.MapCarriers(policy);
                _policyService.MapCoverages(policy);
                _policyService.MapProductsOfCoverage(policy, policy.CoverageId);
                ViewBag.Message = "Error while updating policy details";
                return View(policy);
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

        // GET: Policies
        public ActionResult PolicyBudget(int id,int? year)
        {
            var policyBudget = _policyService.GetAllPolicyBudgets(id);
            ViewBag.Years = DateUtil.GetPreviousYears(5);
            if(year!=null && year > 0)
            {
                policyBudget = policyBudget.Where(pb => pb.Year == year).ToList();
            }
            return View(policyBudget);
        }

        [HttpGet]
        public ActionResult AddBudget(int policyId, int clientId,int ClientPolicyId)
        {
            var client = _clinetService.GetById(clientId);
            var policy = _policyService.GetById(policyId);
            var coverage = _commonService.GetCoverageById(policy.CoverageId);
            var product = _commonService.GetCoverageById(policy.ProductId);
            ViewBag.Years = DateUtil.GetPreviousYears(5);
            var model = new AddPolicyBudget()
            {
                ClientPolicyId= ClientPolicyId,
                ClientId = clientId,
                ClientName = client.Name,
                PolicyId= policyId,
                PolicyNumber = policy.PolicyNumber,
                Coverage = coverage.Name,
                Product = product.Name
            };
            return View(model);
        }
        [HttpPost]
        public ActionResult AddBudget(AddPolicyBudget projectBudget)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = _commonService.AddClientPolocyBudget(projectBudget);

                    if (result)
                    {
                        ViewBag.Message = "budget details added successfully";
                    }
                }
                ViewBag.Years = DateUtil.GetPreviousYears(5);
                return View(projectBudget);
            }
            catch (Exception ex)
            {
                ViewBag.Years = DateUtil.GetPreviousYears(5);
                ViewBag.Message = "Error while adding budget details";
                return View(projectBudget);
            }
        }

        [HttpGet]
        public ActionResult EditBudget(int policyId, int clientId, int year)
        {
            var budget = _commonService.GetAllPolicyBudgetsForClientPolicyYear(clientId, policyId, year);
           
            var client = _clinetService.GetById(clientId);
            var policy = _policyService.GetById(policyId);
            var coverage = _commonService.GetCoverageById(policy.CoverageId);
            var product = _commonService.GetCoverageById(policy.ProductId);
            ViewBag.Years = DateUtil.GetPreviousYears(5);

            //var model = new AddPolicyBudget()
            //{
            //    ClientId = clientId,
            //    ClientName = client.Name,
            //    PolicyId = policyId,
            //    PolicyNumber = policy.PolicyNumber,
            //    Coverage = coverage.Name,
            //    Product = product.Name
            //};

            budget.ClientId = clientId;
            budget.ClientName = client.Name;
            budget.PolicyId = policyId;
            budget.PolicyNumber = policy.PolicyNumber;
            budget.Coverage = coverage.Name;
            budget.Product = product.Name;
            budget.Year = year;
            return View(budget);
        }
        [HttpPost]
        public ActionResult EditBudget(AddPolicyBudget projectBudget)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = _commonService.UpdateClientPolocyBudget(projectBudget);

                    if (result)
                    {
                        return RedirectToAction("PolicyBudget", new { id = projectBudget.PolicyId });
                    }
                }

                return View(projectBudget);
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Error while updated budget details";
                return View(projectBudget);
            }
        }
        [HttpGet]
        public JsonResult GetProducts(int coverageId)
        {
            var products = _commonService.GetAllProductsByCoverageId(coverageId);
            return Json(products, JsonRequestBehavior.AllowGet);
        }

    }
}