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
    public class BudgetController : Controller
    {
        private readonly IPolicyService _policyService;
        private readonly ICarrierService _carrierService;
        private readonly ICommonService _commonService;
        private readonly IClientService _clinetService;

        public BudgetController(IPolicyService policyService, ICarrierService carrierService,
            ICommonService commonService, IClientService clinetService)
        {
            _policyService = policyService;
            _carrierService = carrierService;
            _commonService = commonService;
            _clinetService = clinetService;
        }
        // GET: Budget
       public ActionResult Index(int clientId, int policyId, int year)
        {
            var ClientPolicy = _policyService.GetAllPolicyBudgetsForClientPolicyYear(clientId, policyId, year);
            var years = DateUtil.GetPreviousYears(10);
            ViewBag.Years = years;

            var clients = _clinetService.GetAllClients();
            ViewBag.Clients = clients;
            return View(ClientPolicy);
        }

       public ActionResult PolicyBudgets(int policyId, int clientId, int year)
        {
            var clientPolicy = _policyService.GetAllPolicyBudgetsForClientPolicyYear(clientId, policyId, year);
            var years = DateUtil.GetPreviousYears(5);
            ViewBag.Years = years;

            var clients = _clinetService.GetAllClients();
            ViewBag.Clients = clients;
            return View(clientPolicy);
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
    }
}