using IBS.Core.Entities;
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
        private readonly IPolicyService _policyService;
        private readonly ICommisionService _commisionService;
        private readonly ICommonService _commonService;
        private readonly IClientService _clientService;
        public CommisionController(ICarrierService carrierService, ICommisionService commisionService, 
            ICommonService commonService,IClientService clientService, IPolicyService policyService)
        {
            _carrierService = carrierService;
            _commisionService = commisionService;
            _commonService = commonService;
            _clientService = clientService;
            _policyService = policyService;
        }
        // GET: Commision
        public ActionResult Index(int? carrierId, bool? isSaved)
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
            var savedCommisions = commisions.Where(cpm => cpm.ProductId != null && cpm.ProductId > 0 && cpm.StatementDate != null && cpm.CommisionValue != null && cpm.CommisionValue > 0 && cpm.PaymentId != null).ToList();
            savedCommisions.ForEach(c =>
            {
                var coverage = _commisionService.GetAllCorporateProducts().ToList().FirstOrDefault(cp => cp.Id == c.ProductId);
                var corporateProducts = _commonService.GetAllCorporateXProducts().ToList().FirstOrDefault(cp => cp.CorporateProductId == c.ProductId);
                if (corporateProducts != null)
                {
                    var policy = _commisionService.GetPolicyByNoCarriageCoverage(c.PolicyNumber, c.CarrierId, coverage.CoverageId);
                    var clientPolicy = _commisionService.GetClientPoliciesByPolicyId(policy.Id);
                    c.ClientPolicyId = clientPolicy.Id;
                }


            });
            _commisionService.SaveCommisions(savedCommisions, false);
            //return RedirectToAction("Index", new { carrierId = commisions[0].CarrierId, isSaved=true });
            return Json(_commisionService.GetCarrierPoliciesById(1), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        // post: Commision
        public ActionResult ProductsOfPolicy(string client, string policyNo, string coverage)
        {
            var products = _commisionService.GetProductsOfPolicy(client, policyNo, coverage);
            products = products.Distinct().ToList();
            return Json(products, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditCommissions(int? carrierId, string smd, string pId, bool? isSaved)
        {
            var commisions = new List<CommisionModel>();
            ViewBag.carrierStatementDates = new List<SelectListCommon>();
            ViewBag.carrierStatementDatePayments = new List<SelectListCommon>();

            if (carrierId != null)
            {
                ViewBag.carrierStatementDates = _commisionService.GetCarrierStatementDates(Convert.ToString(carrierId));
            }

            if (!string.IsNullOrEmpty(smd))
            {
                ViewBag.carrierStatementDatePayments = _commisionService.GetCarrierStatementDatePayments(Convert.ToString(carrierId), smd);
            }
            try
            {
                ViewBag.Carriers = _carrierService.GetAllCarriers();

                if (carrierId != null && carrierId > 0)
                {
                    ViewBag.Status = CommonUtil.GetStatus();
                    commisions = _commisionService.GetAllSavedCommissionsForCarrier(Convert.ToInt32(carrierId));
                }
                ViewBag.PersistMessage = isSaved != null && isSaved == true ? "Commissions updated successfully" : "";
                commisions = commisions.Where(c => c.CarrierId == carrierId).ToList();
                if (!string.IsNullOrEmpty(smd) && commisions != null && commisions.Count > 0 && smd != "-- Please select a statement date --")
                {
                    commisions = commisions.Where(c => c.StatementDateAsString == smd).ToList();
                }
                if (!string.IsNullOrEmpty(pId) && commisions != null && commisions.Count > 0 && pId != "-- Please select a paymentid --")
                {
                    commisions = commisions.Where(c => c.PaymentId.ToLower().TrimEnd() == pId.ToLower().TrimEnd()).ToList();
                }
                return View(commisions);
            }
            catch (Exception ex)
            {
                return View(commisions);
            }

        }

        [HttpPost]
        // post: Commision
        public ActionResult EditCommissions(List<CommisionModel> commisions)
        {

            _commisionService.SaveCommisions(commisions, true);
            return Json(_commisionService.GetCarrierPoliciesById(1), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        // post: Commision
        public ActionResult carrierStatementDates(string carId)
        {
            var smDates = _commisionService.GetCarrierStatementDates(carId);

            smDates = smDates.Distinct().ToList();
            return Json(smDates, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        // post: Commision
        public ActionResult carrierStatementDatePaymentNo(string carId, string smId)
        {
            var payments = _commisionService.GetCarrierStatementDatePayments(carId, smId);
            payments = payments.Distinct().ToList();
            return Json(payments, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        // post: Commision
        public ActionResult GetCoverage(int productId)
        {
            var covId = _commonService.GetCorporateProductById(productId).CoverageId;
            var coverage = _commonService.GetCoverageById(covId);
            return Json(coverage, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        // post: Commision
        public ActionResult DeleteCommision(int commissionId)
        {
            var covId = _commonService.DeleteCommission(commissionId);
            return Json(_commonService.GetCoverageById(1), JsonRequestBehavior.AllowGet);
        }

        public ActionResult ReconcilationCommissions(int? type,int? carrierId, string smd, string pId, bool? isSaved)
        {
            var commisions = new List<CommisionModel>();
            ViewBag.carrierStatementDates = new List<SelectListCommon>();
            ViewBag.carrierStatementDatePayments = new List<SelectListCommon>();

            if (carrierId != null)
            {
                ViewBag.carrierStatementDates = _commisionService.GetCarrierStatementDates(Convert.ToString(carrierId));
            }

            if (!string.IsNullOrEmpty(smd))
            {
                ViewBag.carrierStatementDatePayments = _commisionService.GetCarrierStatementDatePayments(Convert.ToString(carrierId), smd);
            }
            try
            {
                //ViewBag.Carriers = _carrierService.GetAllCarriers();

                if (carrierId != null && carrierId > 0)
                {
                    ViewBag.Status = CommonUtil.GetStatus();
                    commisions = _commisionService.GetAllSavedCommissionsForCarrier(Convert.ToInt32(carrierId));
                }
                ViewBag.PersistMessage = isSaved != null && isSaved == true ? "Commissions updated successfully" : "";
                commisions = commisions.Where(c => c.CarrierId == carrierId).ToList();
                if (!string.IsNullOrEmpty(smd) && commisions != null && commisions.Count > 0 && smd != "-- Please select a statement date --")
                {
                    commisions = commisions.Where(c => c.StatementDateAsString == smd).ToList();
                }
                var savedCommissions = _commisionService.GetSavedCommisions();
                if (!string.IsNullOrEmpty(pId) && commisions != null && commisions.Count > 0 && pId != "-- Please select a paymentid --")
                {
                    commisions = commisions.Where(c => c.PaymentId == pId).ToList();
                }

                if (type == 1)
                {
                    
                    commisions = commisions.Where(c => c.ReconsilationStatus == "Open").ToList();
                    savedCommissions = savedCommissions.Where(c => c.ReconcilationStatus == null).ToList();
                }
                if (type == 2)
                {
                    commisions = commisions.Where(c => c.ReconsilationStatus == "Verified").ToList();
                    savedCommissions = savedCommissions.Where(c => c.ReconcilationStatus == "Verified").ToList();
                }

               
                if(savedCommissions != null && savedCommissions.Count > 0)
                {
                    var vbCarriers = new List<CarrierModel>();
                    var carriers = _carrierService.GetAllCarriers();
                    var cpIds = savedCommissions.Select(c => c.ClientPolicyId).ToList();
                    cpIds.ForEach(cp =>
                    {
                        var clientPolicy = _commisionService.GetAllClientPoliciesByIndexId(cp);
                        var policy = _policyService.GetById(clientPolicy.PolicieId);

                        var carrier = carriers.FirstOrDefault(c => c.Id == policy.CarId);
                        if(vbCarriers.FirstOrDefault(vbc=>vbc.Id==carrier.Id)==null)
                        vbCarriers.Add(carrier);

                    });
                    ViewBag.Carriers = vbCarriers;
                }
                else
                {
                    ViewBag.Carriers = new List<CarrierModel>();
                }
                

                return View(commisions);
            }
            catch (Exception ex)
            {                      
                return View(commisions);
            }

        }

        [HttpPost]
        // post: Commision
        public ActionResult ReconcilationCommissions(List<CommisionModel> commisions)
        {

            _commisionService.UpdateCommisions(commisions);
            return Json(_commisionService.GetCarrierPoliciesById(1), JsonRequestBehavior.AllowGet);
        }

        public ActionResult ExceptionCommissions(int? carrierId, string smd)
        {
            ViewBag.Carriers = _commisionService.GetExceptionCommissionsCariers();
            ViewBag.carrierStatementDates = _commisionService.GetExceptionCarrierStatementDates(carrierId);   
            var commisions = _commisionService.GetAllExceptionCommissionsForCarrier(carrierId);

            if (!string.IsNullOrEmpty(smd) && commisions != null && commisions.Count > 0 && smd != "-- Please select a statement date --")
            {
                commisions = commisions.Where(c => c.StatementDateAsString == smd).ToList();
            }

            return View(commisions);
        }
        [HttpPost]
        public ActionResult ExceptionCommissions(List<ExceptionCommissionModel> commisions)
        {
            var commissionModels = new List<CommisionModel>();
            var exceptionCommissionModels = new List<ExceptionCommissionModel>();
            commisions.ForEach(c =>
            {
                if(!string.IsNullOrEmpty(c.ClientName) &&
                c.ClientPolicyId>0 &&
                c.CommissionValue > 0)
                {
                    commissionModels.Add(new CommisionModel()
                    {
                        ClientPolicyId=c.ClientPolicyId,
                        CommisionValue=c.CommissionValue,
                        CommisionString=c.CommissionValue.ToString(),
                        IsExceptionCommission=true,
                        AppliedDate=c.AppliedDate,
                        StatementDate=c.StatementDate,
                        PaymentId=c.PaymentId
                    });
                    exceptionCommissionModels.Add(new ExceptionCommissionModel()
                    {
                        Id=c.Id
                    });
                }
            });
            _commisionService.SaveCommisions(commissionModels, true);
            _commisionService.UpdateExceptionCommisions(exceptionCommissionModels);
            return Json(_commisionService.GetCarrierPoliciesById(1), JsonRequestBehavior.AllowGet);
        }
        public ActionResult AssignClient(int Id,string policyNumber,int policyId,string carId,string smd)
        {
            ViewBag.Clients = _clientService.GetAllClients();
            var model = new AssignClientToPolicy()
            {
                Id = Id,
                PolicyNo = policyNumber,
                PolicyId= policyId,
                CarrierId=carId,
                StatementDate=smd
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult AssignClient(AssignClientToPolicy clientPolicy)
        {
            var clinetPolicy = new ClientPolicie()
            {
                ClientId = clientPolicy.ClinetId,
                PolicieId = clientPolicy.PolicyId
            };
            _commonService.AddClientPolocie(clinetPolicy);
            
            _commisionService.UpdateExceptionCommisionsClient(clientPolicy.Id, clientPolicy.ClinetId, clientPolicy.PolicyId,clientPolicy.PolicyNo);
            return Json(_commisionService.GetCarrierPoliciesById(1), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        // post: Commision
        public ActionResult ExceptionCarrierStatementDates(int carId)
        {
            var smDates = _commisionService.GetExceptionCarrierStatementDates(carId);

            smDates = smDates.Distinct().ToList();
            return Json(smDates, JsonRequestBehavior.AllowGet);
        }
    }
}