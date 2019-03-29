using IBS.Core.Models;
using IBS.Service.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IBS.Controllers
{
    public class ClientController : System.Web.Mvc.Controller
    {
        private readonly IClientService _clientService;
        private readonly IPolicyService _polocyService;
        public ClientController(IClientService clientService, IPolicyService polocyService)
        {
            _clientService = clientService;
            _polocyService = polocyService;
        }
        // GET: Client
        public ActionResult Index()
        {
            var clients = _clientService.GetAllClients();
            return View(clients);
        }
        // GET: Client/AddClient  
        public ActionResult AddClient()
        {
            var client = new ClientModel();

            return View(client);
        }

        // POST: Client/AddClient    
        [HttpPost]
        public ActionResult AddClient(ClientModel client)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = _clientService.AddClient(client);

                    if (result)
                    {
                        ViewBag.Message = "Client details added successfully";
                    }
                }

                return View(client);
            }
            catch
            {
                return View(client);
            }
        }

        // GET: Client/EditClientDetails/1   
        public ActionResult EditClientDetails(int id)
        {
            var client = _clientService.GetById(id);
            return View(client);
        }

        // POST: Client/EditClientDetails/1   
        [HttpPost]

        public ActionResult EditClientDetails(int id, ClientModel obj)
        {
            try
            {
                _clientService.ModifyClient(obj);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Client/DeleteClient/1    
        public ActionResult DeleteClient(int id)
        {
            try
            {
                var result = _clientService.DeleteClient(id);
                if (result)
                {
                    ViewBag.AlertMsg = "Client details deleted successfully";
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Details(int id)
        {
            var client = _clientService.GetAllClientPolocies(id);
            var policies = _polocyService.GetAllPolicies();

            var viewPolocies = new List<PolicyModel>();
            policies.ToList().ForEach(p =>
            {
                var isEixst = client.Polocies.FirstOrDefault(cp => cp.Id == p.Id);
                if (isEixst == null)
                {
                    viewPolocies.Add(p);
                }
            });
            ViewBag.Polocies = viewPolocies;

            return View(client);
        }

        [HttpPost]
        public JsonResult AddClientPolocie(int clientId,int polocieId)
        {
            _clientService.AddClientPolocie(clientId, polocieId);
            return Json(_clientService.GetAllClients(), JsonRequestBehavior.AllowGet);
        }
    }
}