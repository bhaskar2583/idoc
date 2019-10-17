using IBS.Core.Enums;
using IBS.Core.Models;
using IBS.Service.Helpers;
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
            _policyService.MapClients(policy);
            return View(policy);
        }

        [HttpPost]
        public ActionResult GetPatrners()
        {
            var Partners = _carrierService.GetAllCarriers().ToList();
            return Json(Partners, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetDivisions()
        {

            return Json(ListHelper.GetDivisions(), JsonRequestBehavior.AllowGet);
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
                _policyService.MapClients(policy);
                _policyService.MapProductsOfCoverage(policy, policy.CoverageId);
                return View(policy);
            }
            catch (Exception ex)
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
            _policyService.MapProductsOfCoverage(policy, policy.SelectedCoverage.Id);
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
            catch(Exception ex)
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
        private PolicyBudgetsModel MapCarrier(PolicyBudgetsModel PB)
        {
            var policy = _policyService.GetById(PB.PolicyId);
            var client = _clinetService.GetById(PB.ClientId);
            var carrier = _carrierService.GetById(policy.CarId);
            PB.CarrierName = carrier?.Name;
            PB.ClientName = client?.Name;
            return PB;
        }
        private PolicyBudgetsModel MapCarrierAction(Func<PolicyBudgetsModel, PolicyBudgetsModel> executor, PolicyBudgetsModel model)
        {
           return executor(MapCarrier(model));
        }
        // GET: Policies
        public ActionResult PolicyBudgets(int policyId, int clientId, int year,int? carId,int? divId)
        {

            var ClientPolicy = _policyService.GetAllPolicyBudgetsForClientPolicyYear(clientId, policyId, year);

            if (carId > 0)
            {
                ClientPolicy = ClientPolicy.Where(cp => cp.CarId == carId).ToList();
            }
            if (divId > 0)
            {
                ClientPolicy = ClientPolicy.Where(cp => cp.DivisionId == divId).ToList();
            }
            ClientPolicy.ToList().ForEach(cp =>
            {
               cp= MapCarrierAction(MapCarrier, cp);
            });
            var years = DateUtil.GetPreviousYears(5);
            ViewBag.Years = years;

            var clients = _clinetService.GetAllClients();
            ViewBag.Clients = clients;
            return View(ClientPolicy);

        }
        // GET: Policies
        public ActionResult PolicyBudget(int id, int? year)
        {
            int? yearValue = 0;
            if (year != null)
            {
                yearValue = year;
            }
            var ClientPolicy = _commonService.GetClientPoliciesByPolicyId(id);
            return RedirectToAction("AddBudget", new { policyId = id, clientId = ClientPolicy.ClientId, ClientPolicyId = 0, year = yearValue });

        }

        [HttpGet]
        public ActionResult AddBudget(int policyId, int clientId, int ClientPolicyId, int year = 0)
        {
            var data = new List<AddPolicyBudget>();
            var allClients = _clinetService.GetAllClients().OrderBy(c=>c.Name);
            ViewBag.Clients = allClients;

            var years = DateUtil.GetPreviousYearsSelectList(5);
            ViewBag.Years = years;// new SelectList(years, "Id", "Name");

            if (clientId == 0 || year == 0)
                return View(data);
            //var client = _clinetService.GetById(clientId);

            //var cpDetails = _commonService.GetAllClientPolicies().Where(c => c.ClientId == clientId).OrderByDescending(cp => cp.Id);
            //var pDetails = _policyService.GetById(policyId);
            //var carrierProduct = _commonService.GetAllCorporateXProducts().FirstOrDefault(cp => cp.ProductId == pDetails.ProductId);
            //cpDetails.ToList().ForEach(cp =>
            //{
            //    var pDetailsChild = _policyService.GetById(cp.PolicieId);
            //    var carrierProductChild = _commonService.GetAllCorporateXProducts()
            //    .FirstOrDefault(cpp => cpp.ProductId == pDetailsChild.ProductId);

            //    if (carrierProductChild != null && carrierProduct.CorporateProductId == carrierProductChild.CorporateProductId
            //    && pDetailsChild.PolicyNumber == pDetails.PolicyNumber
            //    && pDetailsChild.CarId == pDetails.CarId)
            //    {
            //        policyId = pDetailsChild.Id;

            //    }
            //});

            //var policy = _policyService.GetById(policyId);

            //var coverage = _commonService.GetCoverageById(policy.CoverageId);
            //var product = _commonService.GetProductById(policy.ProductId);

            //var yearsSelectItems = new List<SelectListItem>();
            //var years = DateUtil.GetPreviousYearsSelectList(5);

            //ViewBag.Years = new SelectList(years, "Id", "Name");

            //var policyBudget = _policyService.GetAllPolicyBudgets(policyId);
            //if (year > 0)
            //{
            //    policyBudget = policyBudget.Where(pb => pb.Year == year).ToList();
            //}
            //else
            //{
            //    year = years.FirstOrDefault().Id;
            //    policyBudget = policyBudget.Where(pb => pb.Year == year).ToList();
            //}

            //var model = new AddPolicyBudget()
            //{
            //    ClientPolicyId = ClientPolicyId,
            //    ClientId = client.Id,
            //    ClientName = client?.Name,
            //    PolicyId = policyId,
            //    PolicyNumber = policy.PolicyNumber,
            //    Coverage = coverage.Name,
            //    Product = product.Name,
            //    Year = year
            //};

            //if (policyBudget != null && policyBudget.Count > 0)
            //{
            //    model.JanBudget = policyBudget[0].JanBudget;
            //    model.FebBudget = policyBudget[0].FebBudget;

            //    model.MarchBudget = policyBudget[0].MarchBudget;

            //    model.AprilBudget = policyBudget[0].AprilBudget;

            //    model.MayBudget = policyBudget[0].MayBudget;

            //    model.JunBudget = policyBudget[0].JunBudget;

            //    model.JulyBudget = policyBudget[0].JulyBudget;

            //    model.AugBudget = policyBudget[0].AugBudget;

            //    model.SepBudget = policyBudget[0].SepBudget;

            //    model.OctBudget = policyBudget[0].OctBudget;
            //    model.NovBudget = policyBudget[0].NovBudget;

            //    model.DecBudget = policyBudget[0].DecBudget;
            //    model.TotalBudget = model.JanBudget + model.FebBudget + model.MarchBudget + model.AprilBudget + model.MayBudget + model.JunBudget + model.JulyBudget + model.AugBudget + model.SepBudget + model.OctBudget + model.NovBudget + model.DecBudget;

            //}
            //else
            //{
            //    model.TotalBudget = 0.00M;
            //    model.JanBudget = model.FebBudget = model.MarchBudget = model.AprilBudget = model.MayBudget = model.JunBudget = model.JulyBudget = model.AugBudget = model.SepBudget = model.OctBudget = model.NovBudget = model.DecBudget = 0.00M;
            //}
            //return View(model);

            var clients = new List<ClientModel>();
            var client = _clinetService.GetById(clientId);
            clients.Add(client);
            clients.ToList().ForEach(cli =>
            {
                var cpDetails = _commonService.GetAllClientPolicies().Where(c => c.ClientId == cli.Id).OrderByDescending(cp => cp.Id);


                cpDetails.ToList().ForEach(cp1 =>
                {
                    //  var pDetails = _policyService.GetById(cp1.PolicieId);
                    // var carrierProduct = _commonService.GetAllCorporateXProducts().FirstOrDefault(cp => cp.ProductId == pDetails.ProductId);
                    // var pDetailsChild = _policyService.GetById(cp1.PolicieId);
                   

                    //if (carrierProductChild != null && carrierProduct.CorporateProductId == carrierProductChild.CorporateProductId
                    //&& pDetailsChild.PolicyNumber == pDetails.PolicyNumber
                    //&& pDetailsChild.CarId == pDetails.CarId)
                    //{
                    //    policyId = pDetailsChild.Id;

                    //}

                    var policy = _policyService.GetById(cp1.PolicieId);
                    var coverage = _commonService.GetCoverageById(policy.CoverageId);
                    var car = _carrierService.GetById(policy.CarId);
                    var product = _commonService.GetProductById(policy.ProductId);
                    var carrierProductChild = _commonService.GetAllCorporateXProducts()
                   .FirstOrDefault(cpp => cpp.ProductId == product.Id);

                    if (policy == null || coverage == null || car == null || product == null || carrierProductChild==null)
                        return;

                    

                    var policyBudget = _policyService.GetAllPolicyBudgets(policy.Id);
                    if (year > 0)
                    {
                        policyBudget = policyBudget.Where(pb => pb.Year == year).ToList();
                    }
                    if (data.Any(d => d.PolicyNumber == policy.PolicyNumber
                     && d.ClientId == cli.Id
                     && d.CarId == policy.CarId
                     && d.CoverageId == policy.CoverageId
                     && d.CProductId == carrierProductChild.Id
                    ))
                        return;

                    var model = new AddPolicyBudget()
                    {
                        ClientPolicyId = cp1.Id,
                        ClientId = cli.Id,
                        ClientName = cli?.Name,
                        PolicyId = policy.Id,
                        PolicyNumber = policy.PolicyNumber,
                        CoverageId=coverage.Id,
                        Coverage = coverage.Name,
                        ProductId=product.Id,
                        Product = product.Name,
                        CProductId= carrierProductChild.Id,
                        Year = year,
                        CarId= car.Id,
                        CarName= car.Name
                    };

                    if (policyBudget != null && policyBudget.Count > 0)
                    {
                        model.JanBudget = policyBudget[0].JanBudget;
                        model.FebBudget = policyBudget[0].FebBudget;

                        model.MarchBudget = policyBudget[0].MarchBudget;

                        model.AprilBudget = policyBudget[0].AprilBudget;

                        model.MayBudget = policyBudget[0].MayBudget;

                        model.JunBudget = policyBudget[0].JunBudget;

                        model.JulyBudget = policyBudget[0].JulyBudget;

                        model.AugBudget = policyBudget[0].AugBudget;

                        model.SepBudget = policyBudget[0].SepBudget;

                        model.OctBudget = policyBudget[0].OctBudget;
                        model.NovBudget = policyBudget[0].NovBudget;

                        model.DecBudget = policyBudget[0].DecBudget;
                        model.TotalBudget = model.JanBudget + model.FebBudget + model.MarchBudget + model.AprilBudget + model.MayBudget + model.JunBudget + model.JulyBudget + model.AugBudget + model.SepBudget + model.OctBudget + model.NovBudget + model.DecBudget;

                    }
                    else
                    {
                        model.TotalBudget = 0.00M;
                        model.JanBudget = model.FebBudget = model.MarchBudget = model.AprilBudget = model.MayBudget = model.JunBudget = model.JulyBudget = model.AugBudget = model.SepBudget = model.OctBudget = model.NovBudget = model.DecBudget = 0.00M;
                    }
                    data.Add(model);
                });

                
            });


            return View(data);
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
                else
                {
                    ViewBag.Message = "Error while adding budget details";
                }
                var years = DateUtil.GetPreviousYearsSelectList(5);
                ViewBag.Years = new SelectList(years, "Id", "Name");
                var client = _clinetService.GetById(projectBudget.ClientId);
                projectBudget.ClientName = client.Name;
                return View(projectBudget);
            }
            catch (Exception ex)
            {
                var years = DateUtil.GetPreviousYearsSelectList(5);
                ViewBag.Years = new SelectList(years, "Id", "Name");
                ViewBag.Message = "Error while adding budget details";
                var client = _clinetService.GetById(projectBudget.ClientId);
                projectBudget.ClientName = client.Name;
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