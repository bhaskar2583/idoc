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
        public ClientController(IClientService clientService)
        {
            _clientService = clientService;
        }
        // GET: Carrier
        public ActionResult Index()
        {
            var clients = _clientService.GetAllClients();
            return View(clients);
        }
        // GET: Client/AddClient  
        public ActionResult AddClient()
        {
            return View();
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

                return View();
            }
            catch
            {
                return View();
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
    }
}