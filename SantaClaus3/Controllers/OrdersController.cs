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
    public class OrdersController : Controller
    {
        // GET: Order
        public ActionResult Index()
        {
            SantaClaus3MongoDB db = new SantaClaus3MongoDB();
            var requests_kids = db.GetAllRequestKid();
            Orders model = new Orders();
            model.EntityList = requests_kids.ToList();
            return View(model);
        }

        public ActionResult Details(string id)
        {
            SantaClaus3MongoDB db = new SantaClaus3MongoDB();
            var request_kid = db.GetRequest(id);
            Orders model = new Orders();
            ViewBag.Id = request_kid.ID;
            ViewBag.KidName = request_kid.KidName;
            ViewBag.Date = request_kid.Date.ToString("dd-MMM-yyyy");
            switch (request_kid.Status.ToString())
            {
                case "0":
                    ViewBag.Status = "In Progress";
                    break;
                case "1":
                    ViewBag.Status = "Available";
                    break;
                case "2":
                    ViewBag.Status = "Done";
                    break;

                default:
                    break;
            }

            model.ToyList = request_kid.ToyKids;

            return View(model);
        }


        public ActionResult Edit(string id)
        {
            SantaClaus3MongoDB db = new SantaClaus3MongoDB();
            var request_kid = db.GetRequest(id);
            //utile per estrarre tutti i giochi richiesto dal bambino
            Orders modelToy = new Orders();
            modelToy.ToyList = request_kid.ToyKids;
            Toy toy = new Toy();
            //utile per passare alla view lo stato dell ordine
            Order model = new Order();
            model.Status = request_kid.Status;
            return View(model);
        }

        public ActionResult Save(int status, string id)
        {
            if (string.IsNullOrWhiteSpace(status.ToString()))
            {
                throw new MissingFieldException("name cannot be null");
            }
            bool result;
            SantaClaus3MongoDB db = new SantaClaus3MongoDB();
            var request_kid = db.GetRequest(id);
            Orders modelToy = new Orders();
            modelToy.ToyList = request_kid.ToyKids;
            Toy toy = new Toy();
            var query = modelToy.ToyList.GroupBy(x => x)
                                .Select(y => new { Element = y.Key, Counter = y.Count() })
                                .ToList();
            foreach (var toyRequest in query)
            {
                toy = db.GetToy(toyRequest.Element.ToyName);
                if (toy.Amount <= toyRequest.Counter)
                {
                    ModelState.AddModelError("", "Order no Avaible");
                    return RedirectToAction("Details", id);
                }
            }

            if (string.IsNullOrWhiteSpace(id))
            {
                Order requestkid = new Order();

            }

            result = db.UpdateStatus(new Order
            {
                ID = id,
                Status = status
            });


            foreach (var toyRequest in modelToy.ToyList)
            {

                toy = db.GetToy(toyRequest.ToyName);
                result = db.UpdateAmountToy(toy);
                if (toy.Amount == 0)
                {
                    db.RemoveToy(toy.ID);
                }
            }

            return RedirectToAction("Index", new { result = result });
        }
    }
}