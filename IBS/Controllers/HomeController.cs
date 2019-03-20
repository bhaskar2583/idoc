using IBS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IBS.Controllers
{
    public class HomeController : Controller
    {

        DataContext db = new DataContext();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AddCarrier()
        {
            return View();
        }

        public ActionResult CarrierDetails(int id)
        {
            DataContext carrierdetails = new DataContext();
            AddCarrier x = carrierdetails.Carriers.Single(carr => carr.Id == id);
            return View(x);
        }

        public ActionResult AddClient()
        {
            return View();
        }

        public ActionResult AddPolicy()
        {
            return View();
        }
    }
}