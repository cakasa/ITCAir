using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITCAir.Data;
using ITCAir.Web.Attributes;
using ITCAir.Web.Models.Flights;
using ITCAir.Web.Models.Reservations;
using ITCAir.Web.Models.Shared;
using Microsoft.AspNetCore.Mvc;

namespace ITCAir.Web.Controllers
{
    public class ReservationsController : Controller
    {
        private readonly ITCAirContext context;
        private const int PageSize = 10;

        public ReservationsController()
        {
            context = new ITCAirContext();
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ReservationDate]
        public IActionResult ProcessFirstStepReservation(FirstStepReservationModel model, bool oneWay)
        {
            if (ModelState.IsValid)
            {
                GetViewData( model);

                model.OneWay = oneWay;
                return View("ReservationsFlights", model);
            }
            return View("Index");
        }

        private void GetViewData(FirstStepReservationModel model)
        {
            var now = new AllFlightsViewModel();
            now.Pager ??= new PagerViewModel();
            now.Pager.CurrentPage = now.Pager.CurrentPage <= 0 ? 1 : now.Pager.CurrentPage;

            List<FlightViewModel> items = context.Flights.Skip((now.Pager.CurrentPage - 1) * PageSize).Take(PageSize).Select(c => new FlightViewModel()
            {
                Id = c.Id,
                From = c.From,
                To = c.To,
                Departure = c.DepartureTime,
                Arrival = c.Arrival

            }).Where(c => c.From == model.From && c.To == model.To).ToList();

            now.Items = items;
            now.Pager.PagesCount = (int)Math.Ceiling(context.Flights.Count() / (double)PageSize);
            this.ViewData["AllFlightInfo"] = now;
        }
        public IActionResult ProcessFirstStepReservationOnTop(FirstStepReservationModel model, bool oneWay)
        {
            if (ModelState.IsValid)
            {
                GetViewData(model);
                return View("ReservationsFlights", model);
            }
            return View("ReservationsFlights", model);
        }
    }
}