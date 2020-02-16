using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITCAir.Web.Attributes;
using ITCAir.Web.Models.Reservations;
using Microsoft.AspNetCore.Mvc;

namespace ITCAir.Web.Controllers
{
    public class ReservationsController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ReservationDate]
        public IActionResult ProcessFirstStepReservation(FirstStepReservationModel model,bool oneWay)
        {
            if (ModelState.IsValid)
            {
                model.OneWay = oneWay;
                return View("ReservationsFlights",model);
            }
            return View("Index");
        }
    }
}