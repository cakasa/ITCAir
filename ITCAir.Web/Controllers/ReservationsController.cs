using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITCAir.Data;
using ITCAir.Data.Entities;
using ITCAir.Data.Enum;
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
            ModelClass.SecondModel = default;
            ModelClass.Test = default;
            FlightsClass.GoingPlaneId = default;
            FlightsClass.ReturningPlaneId = default;
        }

        //When the user type the info of the first page
        [HttpPost]
        [UpcomingDate]
        public IActionResult ProcessFirstStepReservation(FirstStepReservationModel model, bool oneWay)
        {
            if (ModelState.IsValid)
            {
                if (model.DepartureDate > model.ReturnDate)
                {
                    ViewData["Error"] = "Arrival cannot be before Departure!";
                    return View("Index");
                }
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

            }).Where(c => c.From == model.To && c.To == model.From && c.Departure >= model.DepartureDate).ToList();

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

            }).Where(c => c.From == model.From && c.To == model.To && c.Departure>=model.DepartureDate).ToList();

            now.GoingFlights = items;
            now.PagerOnGoing.PagesCount = (int)Math.Ceiling(context.Flights.Count() / (double)PageSize);
        }

        public IActionResult ProcessFirstStepReservationOnTop(FirstStepReservationModel model, bool oneWay)
        {
            if (ModelState.IsValid)
            {
                if (model.DepartureDate > model.ReturnDate)
                {
                    ViewData["Error"] = "Arrival cannot be before Departure!";
                    return View("ReservationsFlights", model);

                }
                ModelClass.PassengersCount = model.People;
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

        public IActionResult CheckEmailConfirm(EmailConfirmViewModel confirmEmail)
        {
            if (ConfirmEmail.ValidationToken != confirmEmail.EnteredNumber)
            {
                return RedirectToAction("ThanksForReservation", "Passengers");
            }
            else
            {
                this.context.Reservations.First(r => r.Id == ConfirmEmail.ReservationIdGoing).IsConfirmed = true;
                this.context.SaveChanges();

                if (ConfirmEmail.ReservationIdReturning != 0)
                {
                    this.context.Reservations.First(r => r.Id == ConfirmEmail.ReservationIdReturning).IsConfirmed = true;
                    this.context.SaveChanges();
                }
                ConfirmEmail.EmailConfirmed = true;
            }
            return RedirectToAction("ThanksForReservation", "Passengers");
        }

        public IActionResult ChangePage(int pageId)
        {
            ReservationsFilteringAndPaging.Pager.CurrentPage = pageId;
            return View("AllReservations", GetAllReservations());
        }

        public IActionResult ClearFilter()
        {
            ReservationsFilteringAndPaging.ClearFilter();
            return View("AllReservations", GetAllReservations());
        }

        public IActionResult Filter(AllReservationsViewModel model)
        {
            ReservationsFilteringAndPaging.ClearFilter();
            ReservationsFilteringAndPaging.FilterType = model.FilterType;
            ReservationsFilteringAndPaging.Filter = model.Filter;
            return View("AllReservations", GetAllReservations());
        }

        public IActionResult ChangePageSize(int pageSize)
        {
            ReservationsFilteringAndPaging.Pager.PageSize = pageSize;
            ReservationsFilteringAndPaging.Pager.CurrentPage = 1;
            return View("AllReservations", GetAllReservations());
        }


        public IActionResult AllReservations()
        {
            return View(GetAllReservations(true));
        }

        public AllReservationsViewModel GetAllReservations(bool hasBeenRedirected = false)
        {
            if (hasBeenRedirected)
            {
                ReservationsFilteringAndPaging.Clear();
            }

            var allReservations = this.context.Reservations.ToList();
            List<ReservationViewModel> reservations = new List<ReservationViewModel>();
            if (ReservationsFilteringAndPaging.FilterType != ReservationFilterType.None)
            {
                string filter = ReservationsFilteringAndPaging.Filter;
                switch (ReservationsFilteringAndPaging.FilterType)
                {
                    case ReservationFilterType.Email:
                        allReservations = allReservations.Where(reservation => reservation.Email.Contains(ReservationsFilteringAndPaging.Filter)).ToList();
                        break;
                }
            }

            var pageReservations = allReservations
                                        .Skip((ReservationsFilteringAndPaging.Pager.CurrentPage - 1) * ReservationsFilteringAndPaging.Pager.PageSize)
                                        .Take(ReservationsFilteringAndPaging.Pager.PageSize).ToList();

            ReservationViewModel curReserv = new ReservationViewModel();


            foreach (var reservation in pageReservations)
            {
                curReserv.ReservationId = reservation.Id;
                curReserv.ReservedFlight = reservation.Flight;
                curReserv.ReservationEmail = reservation.Email;
                curReserv.AllPassengers = reservation.Passengers;
                curReserv.Confirmed = reservation.IsConfirmed;

                reservations.Add(curReserv);
                curReserv = new ReservationViewModel();
            }

            AllReservationsViewModel model = new AllReservationsViewModel();
            model.AllReservations = reservations;

            ReservationsFilteringAndPaging.Pager.PagesCount = (int)Math.Ceiling(allReservations.Count() / (double)ReservationsFilteringAndPaging.Pager.PageSize);
            return model;
        }

        public IActionResult AllReservationsDetails(int Id)
        {
            ReservationViewModel reservation = new ReservationViewModel();

            Reservation currentReservation = this.context.Reservations.First(r => r.Id == Id);
            reservation.AllPassengers = currentReservation.Passengers;
            reservation.ReservationEmail = currentReservation.Email;
            reservation.ReservedFlight = currentReservation.Flight;
            reservation.ReservationId = currentReservation.Id;

            return View(reservation);
        }

        public IActionResult DeleteReservation(int Id)
        {
            var curReservation = this.context.Reservations.First(r => r.Id == Id);
            this.context.Reservations.Remove(curReservation);
            this.context.SaveChanges();

            return RedirectToAction("AllReservations");
        }
    }
}