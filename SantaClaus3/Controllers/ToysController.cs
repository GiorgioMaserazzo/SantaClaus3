using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SantaClaus3;
using SantaClaus3.Models;
using SantaClaus3MongoDB = SantaClaus3.MongoDB;

namespace SantaClaus3.Controllers
{
    public class ToysController : Controller
    {
        // GET: Toys
        public ActionResult Index()
        {
            SantaClaus3MongoDB db = new SantaClaus3MongoDB();
            var toys = db.GetAllToys();
            Toys model = new Toys();
            model.EntityList = toys.ToList();

            return View(model);
        }
    }
}