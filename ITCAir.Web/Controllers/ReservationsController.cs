using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITCAir.Data;
using ITCAir.Web.Attributes;
using ITCAir.Web.GlobalConstants;
using ITCAir.Web.Models.Flights;
using ITCAir.Web.Models.Passengers;
using ITCAir.Web.Models.Reservations;
using ITCAir.Web.Models.Shared;
using Microsoft.AspNetCore.Mvc;

namespace ITCAir.Web.Controllers
{
    public class ReservationsController : Controller
    {
        private readonly ITCAirContext context;
        private const int PageSize = 10;

        public ReservationsController(ITCAirContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            ClearStaticClass();
            return View();
        }
        private void ClearStaticClass()
        {
            ModelClass.OneWay = default;
            ModelClass.Passengers = new List<PassengerInfoViewModel>();
            ModelClass.PassengersCount = default;
            ModelClass.SecondModel= default;
            ModelClass.Test= default;
            FlightsClass.GoingPlaneId= default;
            FlightsClass.ReturningPlaneId= default;
        }

        //When the user type the info of the first page
        [HttpPost]
        [UpcomingDate]
        public IActionResult ProcessFirstStepReservation(FirstStepReservationModel model, bool oneWay)
        {
            if (ModelState.IsValid)
            {
                ModelClass.PassengersCount = model.People;
                ModelClass.OneWay = oneWay;
                ModelClass.SecondModel = model;
                GetViewData(model);

                model.OneWay = oneWay;
                return View("ReservationsFlights", model);
            }
            return View("Index");
        }

        private void GetViewData(FirstStepReservationModel model)
        {
            var now = new AllFlightsForReservationsViewModel();

            ProccessGoingFlights(model, now);
            //TODO
            if (ModelClass.OneWay == false)
            {
                ProccessReturning(model, now);
            }


            this.ViewData["AllFlightInfo"] = now;
        }

        private void ProccessReturning(FirstStepReservationModel model, AllFlightsForReservationsViewModel now)
        {
            now.PagerOnReturning ??= new PagerViewModel();
            now.PagerOnReturning.CurrentPage = now.PagerOnReturning.CurrentPage <= 0 ? 1 : now.PagerOnReturning.CurrentPage;

            List<FlightViewModel> items = context.Flights.Skip((now.PagerOnReturning.CurrentPage - 1) * PageSize).Take(PageSize).Select(c => new FlightViewModel()
            {
                Id = c.Id,
                From = c.From,
                To = c.To,
                Departure = c.Departure,
                Arrival = c.Arrival

            }).Where(c => c.From == model.To && c.To == model.From).ToList();

            now.ReturningFlights = items;
            now.PagerOnReturning.PagesCount = (int)Math.Ceiling(context.Flights.Count() / (double)PageSize);
        }

        private void ProccessGoingFlights(FirstStepReservationModel model, AllFlightsForReservationsViewModel now)
        {
            now.PagerOnGoing ??= new PagerViewModel();
            now.PagerOnGoing.CurrentPage = now.PagerOnGoing.CurrentPage <= 0 ? 1 : now.PagerOnGoing.CurrentPage;

            List<FlightViewModel> items = context.Flights.Skip((now.PagerOnGoing.CurrentPage - 1) * PageSize).Take(PageSize).Select(c => new FlightViewModel()
            {
                Id = c.Id,
                From = c.From,
                To = c.To,
                Departure = c.Departure,
                Arrival = c.Arrival

            }).Where(c => c.From == model.From && c.To == model.To).ToList();

            now.GoingFlights = items;
            now.PagerOnGoing.PagesCount = (int)Math.Ceiling(context.Flights.Count() / (double)PageSize);
        }

        public IActionResult ProcessFirstStepReservationOnTop(FirstStepReservationModel model, bool oneWay)
        {
            if (ModelState.IsValid)
            {
                ModelClass.SecondModel = model;
                GetViewData(model);
                return View("ReservationsFlights", model);
            }
            return View("ReservationsFlights", model);
        }

        public IActionResult Details(int Id)
        {
            var currentFlight = this.context.Flights.First(c => c.Id == Id);
            FlightDetailsViewModel flightDetailsViewModel = new FlightDetailsViewModel()
            {
                Id = currentFlight.Id,
                From = currentFlight.From,
                To = currentFlight.To,
                DepartureTime = currentFlight.Departure,
                Arrival = currentFlight.Arrival,
                PlaneId = currentFlight.PlaneId,
                PilotName = currentFlight.PilotName,
                CapacityBusiness = currentFlight.BusinessCapacity,
                CapacityEconomy = currentFlight.EconomyCapacity
            };

            return View(flightDetailsViewModel);
        }

        public IActionResult ReturnFromEdit()
        {
            this.ViewData["ChooseGoing"] = FlightsClass.GoingPlaneId;
            this.ViewData["ChooseReturning"] = FlightsClass.ReturningPlaneId;
            var ReadetModel = ModelClass.SecondModel;
            GetViewData(ReadetModel);
            return View("ReservationsFlights", ReadetModel);
        }

        public IActionResult RedirectToPassangers(int Id)
        {
            return View("ReservationsPassengers");
        }

        public IActionResult BookInfo()
        {
            return View();
        }

        public IActionResult BookForGoind(string Id)
        {
            FlightsClass.GoingPlaneId = int.Parse(Id);
            this.ViewData["ChooseGoing"] = FlightsClass.GoingPlaneId;
            this.ViewData["ChooseReturning"] = FlightsClass.ReturningPlaneId;

            var ReadetModel = ModelClass.SecondModel;
            GetViewData(ReadetModel);
            return View("ReservationsFlights", ReadetModel);
        }

        public IActionResult BookForReturning(string Id)
        {
            FlightsClass.ReturningPlaneId = int.Parse(Id);
            this.ViewData["ChooseReturning"] = FlightsClass.ReturningPlaneId;
            this.ViewData["ChooseGoing"] = FlightsClass.GoingPlaneId;

            var ReadetModel = ModelClass.SecondModel;
            GetViewData(ReadetModel);
            return View("ReservationsFlights", ReadetModel);
        }

        public IActionResult RegisterPassengers()
        {
            return View("PassangersInfo");
        }

    }
}