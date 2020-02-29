using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITCAir.Data;
using ITCAir.Data.Entities;
using ITCAir.Data.Enum;
using ITCAir.Web.GlobalConstants;
using ITCAir.Web.Models.Flights;
using ITCAir.Web.Models.Passengers;
using ITCAir.Web.Models.Reservations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ITCAir.Web.Controllers
{
    [Authorize(Roles = "Admin, User")]
    public class FlightsController : Controller
    {
        private readonly ITCAirContext context;
        public FlightsController(ITCAirContext context)
        {
            this.context = context;
        }

        public IActionResult Flights()
        {
            return View(GetAllUpcomingFlights(true));
        }

        public IActionResult FlightsArchive()
        {
            return View(GetFlightsArchive(true));
        }

        public IActionResult ChangeFlightsPage(int pageId)
        {
            FlightFilteringAndPaging.Pager.CurrentPage = pageId;
            return View("Users", GetAllUpcomingFlights());
        }

        public IActionResult ClearFlightsFilter()
        {
            FlightFilteringAndPaging.ClearFilter();
            return View("Flights", GetAllUpcomingFlights());
        }

        public IActionResult FlightsFilter(AllFlightsViewModel model)
        {
            FlightFilteringAndPaging.ClearFilter();
            FlightFilteringAndPaging.FilterType = model.FilterType;
            FlightFilteringAndPaging.Filter = model.Filter;
            return View("Flights", GetAllUpcomingFlights());
        }

        public IActionResult ChangeFlightsPageSize(int pageSize)
        {
            FlightFilteringAndPaging.Pager.PageSize = pageSize;
            FlightFilteringAndPaging.Pager.CurrentPage = 1;
            return View("Flights", GetAllUpcomingFlights());
        }

        public IActionResult ChangeArchivePage(int pageId)
        {
            FlightFilteringAndPaging.Pager.CurrentPage = pageId;
            return View("FlightsArchive", GetFlightsArchive());
        }

        public IActionResult ClearArchiveFilter()
        {
            FlightFilteringAndPaging.ClearFilter();
            return View("FlightsArchive", GetFlightsArchive());
        }

        public IActionResult ArchiveFilter(AllFlightsViewModel model)
        {
            FlightFilteringAndPaging.ClearFilter();
            FlightFilteringAndPaging.FilterType = model.FilterType;
            FlightFilteringAndPaging.Filter = model.Filter;
            return View("FlightsArchive", GetFlightsArchive());
        }

        public IActionResult ChangeArchivePageSize(int pageSize)
        {
            FlightFilteringAndPaging.Pager.PageSize = pageSize;
            FlightFilteringAndPaging.Pager.CurrentPage = 1;
            return View("FlightsArchive", GetFlightsArchive());
        }

        public AllFlightsViewModel GetFlightsArchive(bool hasBeenRedirected = false)
        {
            if(hasBeenRedirected)
            {
                FlightFilteringAndPaging.Clear();
            }

            List<Flight> validFlights = context.Flights.Where(x => x.Departure <= DateTime.Now).ToList();

            if (FlightFilteringAndPaging.FilterType != FlightFilterType.None)
            {
                string filter = FlightFilteringAndPaging.Filter;
                switch (FlightFilteringAndPaging.FilterType)
                {
                    case FlightFilterType.From:
                        validFlights = validFlights.Where(flight => flight.From.Contains(filter)).ToList();
                        break;
                    case FlightFilterType.To:
                        validFlights = validFlights.Where(flight => flight.To.Contains(filter)).ToList();
                        break;
                }
            }

            var pageFlights = validFlights
                                .Skip((FlightFilteringAndPaging.Pager.CurrentPage - 1) * FlightFilteringAndPaging.Pager.PageSize)
                                .Take(FlightFilteringAndPaging.Pager.PageSize).ToList();

            List<SingleFlightViewModel> flightsWithAppropriateData = new List<SingleFlightViewModel>();
            foreach (var singleFlight in pageFlights)
            {
                SingleFlightViewModel flight = new SingleFlightViewModel()
                {
                    Id = singleFlight.Id,
                    From = singleFlight.From,
                    To = singleFlight.To,
                    Departure = singleFlight.Departure,
                    Duration = singleFlight.Arrival - singleFlight.Departure
                };
                flightsWithAppropriateData.Add(flight);
            }

            
            flightsWithAppropriateData = flightsWithAppropriateData.OrderByDescending(x => x.Departure).ToList();
            AllFlightsViewModel model = new AllFlightsViewModel()
            {
                allFlights = flightsWithAppropriateData
            };

            FlightFilteringAndPaging.Pager.PagesCount = (int)Math.Ceiling(validFlights.Count() / (double)FlightFilteringAndPaging.Pager.PageSize);

            return model;
        }

        public AllFlightsViewModel GetAllUpcomingFlights(bool hasBeenRedirected = false)
        {
            if (hasBeenRedirected)
            {
                FlightFilteringAndPaging.Clear();
            }

            List<Flight> validFlights = context.Flights.Where(x => x.Departure > DateTime.Now).ToList();

            if (FlightFilteringAndPaging.FilterType != FlightFilterType.None)
            {
                string filter = FlightFilteringAndPaging.Filter;
                switch (FlightFilteringAndPaging.FilterType)
                {
                    case FlightFilterType.From:
                        validFlights = validFlights.Where(flight => flight.From.Contains(filter)).ToList();
                        break;
                    case FlightFilterType.To:
                        validFlights = validFlights.Where(flight => flight.To.Contains(filter)).ToList();
                        break;
                }
            }

            var pageFlights = validFlights
                                .Skip((FlightFilteringAndPaging.Pager.CurrentPage - 1) * FlightFilteringAndPaging.Pager.PageSize)
                                .Take(FlightFilteringAndPaging.Pager.PageSize).ToList();

            List<SingleFlightViewModel> flightsWithAppropriateData = new List<SingleFlightViewModel>();
            foreach (var singleFlight in pageFlights)
            {
                SingleFlightViewModel flight = new SingleFlightViewModel()
                {
                    Id = singleFlight.Id,
                    From = singleFlight.From,
                    To = singleFlight.To,
                    Departure = singleFlight.Departure,
                    Duration = singleFlight.Arrival - singleFlight.Departure
                };
                flightsWithAppropriateData.Add(flight);
            }


            flightsWithAppropriateData = flightsWithAppropriateData.OrderBy(x => x.Departure).ToList();
            AllFlightsViewModel model = new AllFlightsViewModel()
            {
                allFlights = flightsWithAppropriateData
            };

            FlightFilteringAndPaging.Pager.PagesCount = (int)Math.Ceiling(validFlights.Count() / (double)FlightFilteringAndPaging.Pager.PageSize);

            return model;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult CreateFlight()
        {
            ViewData["Error"] = string.Empty;
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public IActionResult CreateFlight(CreateFlightViewModel model)
        {
            if (ModelState.IsValid)
            {
                Flight flight = new Flight()
                {
                    From = model.From,
                    To = model.To,
                    BusinessCapacity = model.BusinessCapacity,
                    EconomyCapacity = model.EconomyCapacity,
                    PilotName = model.PilotName,
                    PlaneModel = model.PlanetModel,
                    PlaneId = model.AirplaneId,
                    Arrival = new DateTime(model.DateOfArrival.Year,
                                           model.DateOfArrival.Month,
                                           model.DateOfArrival.Day,
                                           model.TimeOfArrival.Hour,
                                           model.TimeOfArrival.Minute,
                                           0),
                    Departure = new DateTime(model.DateOfDeparture.Year,
                                                 model.DateOfDeparture.Month,
                                                 model.DateOfDeparture.Day,
                                                 model.TimeOfDeparture.Hour,
                                                 model.TimeOfDeparture.Minute,
                                                 0)
                };

                if (flight.Arrival < flight.Departure)
                {
                    ViewData["Error"] = "Arrival cannot be before Departure!";
                    return View(model);
                }
                else if (flight.Departure < DateTime.Now)
                {
                    ViewData["Error"] = "Date of departure should be after current time";
                    return View(model);
                }
                else
                {
                    context.Flights.Add(flight);
                    context.SaveChanges();
                }
            }
            else
            {
                ViewData["Error"] = "Something went wrong";
                return View();
            }
            return View("Flights", GetAllUpcomingFlights(true));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("Flights/Delete/{id}")]
        public IActionResult DeleteFlight(int? id)
        {
            if(context.Flights.Any(x=>x.Id == id))
            {
                Flight flightToRemove = context.Flights.Find(id);
                context.Flights.Remove(flightToRemove);
                context.SaveChanges();
            }

            return View("Flights", GetAllUpcomingFlights());
        }

        public IActionResult ChangeFlightReservationsPageSize(FlightDetailsViewModel model, int pageSize)
        {
            FlightDetailsPaging.Pager.PageSize = pageSize;
            FlightDetailsPaging.Pager.CurrentPage = 1;
        
            return View("FlightDetails", GetFlightDetails());
        }

        public IActionResult ChangeFlightReservationsPage(FlightDetailsViewModel model, int pageId)
        {
            FlightDetailsPaging.Pager.CurrentPage = pageId;
            return View("FlightDetails", GetFlightDetails());
        }

        public FlightDetailsWithPassengersViewModel GetFlightDetails(bool hasBeenRedirected = false)
        {
            if(hasBeenRedirected)
            {
                FlightDetailsPaging.Clear();
            }
            var flight = context.Flights.Find(FlightDetailsPaging.FlightId);

            var model = new FlightDetailsWithPassengersViewModel()
            {
                From = flight.From,
                To = flight.To,
                CapacityBusiness = flight.BusinessCapacity,
                CapacityEconomy = flight.EconomyCapacity,
                PlaneId = flight.PlaneId,
                PilotName = flight.PilotName,
                PlaneModel = flight.PlaneModel,
                Arrival = flight.Arrival,
                Departure = flight.Departure,
            };

            var reservations = context.Reservations.Where(x => x.FlightId == FlightDetailsPaging.FlightId).ToList();
            var pageReservations = reservations
                           .Skip((FlightDetailsPaging.Pager.CurrentPage - 1) * FlightDetailsPaging.Pager.PageSize)
                           .Take(FlightDetailsPaging.Pager.PageSize).ToList();

            var reservationsModel = new List<DetailedFlightReservationViewModel>();
            foreach (var reservation in pageReservations)
            {
                var reservationModel = new DetailedFlightReservationViewModel();
                reservationModel.Email = reservation.Email;

                var passengers = context.Passengers.Where(x => x.ReservationId == reservation.Id);
                var passengersModel = new List<DetailedPassengerInformationViewModel>();
                foreach (var passenger in passengers)
                {
                    var passengerModel = new DetailedPassengerInformationViewModel();
                    passengerModel.FirstName = passenger.FirstName;
                    passengerModel.MiddleName = passenger.MiddleName;
                    passengerModel.LastName = passenger.LastName;
                    passengerModel.Nationality = passenger.Nationality;
                    passengerModel.PersonalId = passenger.PersonalId;
                    passengerModel.PhoneNumber = passenger.Phone;
                    passengerModel.Ticket = passenger.IsBusinessClass ? TicketType.Bussiness : TicketType.Economy;
                    passengersModel.Add(passengerModel);
                }
                reservationModel.Passengers = passengersModel;
                reservationsModel.Add(reservationModel);
            }

            model.Reservations = reservationsModel;
            FlightDetailsPaging.Pager.PagesCount = (int)Math.Ceiling(reservations.Count() / (double)FlightDetailsPaging.Pager.PageSize);
            return model;
        }

        [HttpGet]
        public IActionResult FlightDetails(int? id)
        {    
            if (context.Flights.Any(x => x.Id == id))
            {
                FlightDetailsPaging.FlightId = (int)id;
                var model = GetFlightDetails(true);
                return View(model);
            }
            return View("Flights", GetAllUpcomingFlights());
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult EditFlight(int? id)
        {
            if(context.Flights.Any(x => x.Id == id))
            {
                Flight flightToEdit = context.Flights.Find(id);
                EditFlightViewModel model = new EditFlightViewModel()
                {
                    Id = flightToEdit.Id,
                    From = flightToEdit.From,
                    To = flightToEdit.To,
                    DateOfArrival = flightToEdit.Arrival.Date,
                    TimeOfArrival = flightToEdit.Arrival.TimeOfDay,
                    DateOfDeparture = flightToEdit.Departure.Date,
                    TimeOfDeparture = flightToEdit.Departure.TimeOfDay,
                    PlaneId = flightToEdit.PlaneId,
                    PlaneModel = flightToEdit.PlaneModel,
                    PilotName = flightToEdit.PilotName,
                    EconomyCapacity = flightToEdit.EconomyCapacity,
                    BusinessCapacity = flightToEdit.BusinessCapacity
                };

                ViewData["Error"] = string.Empty;
                return View(model);
            }
            return View("Flights", GetAllUpcomingFlights());
        }

        [HttpPost]
        [Authorize(Roles ="Admin")]
        public IActionResult EditFlight(EditFlightViewModel model)
        {
            if(ModelState.IsValid)
            {
                Flight flightToEdit = context.Flights.Find(model.Id);
                flightToEdit.Id = model.Id;
                flightToEdit.From = model.From;
                flightToEdit.To = model.To;
                flightToEdit.PlaneId = model.PlaneId;
                flightToEdit.PlaneModel = model.PlaneModel;
                flightToEdit.PilotName = model.PilotName;
                flightToEdit.BusinessCapacity = model.BusinessCapacity;
                flightToEdit.EconomyCapacity = model.EconomyCapacity;
                flightToEdit.Arrival = new DateTime(model.DateOfArrival.Year,
                                           model.DateOfArrival.Month,
                                           model.DateOfArrival.Day,
                                           model.TimeOfArrival.Hours,
                                           model.TimeOfArrival.Minutes,
                                           0);
                flightToEdit.Departure = new DateTime(model.DateOfDeparture.Year,
                                                 model.DateOfDeparture.Month,
                                                 model.DateOfDeparture.Day,
                                                 model.TimeOfDeparture.Hours,
                                                 model.TimeOfDeparture.Minutes,
                                                 0);

                if (flightToEdit.Arrival < flightToEdit.Departure)
                {
                    ViewData["Error"] = "Arrival cannot be before Departure!";
                    return View(model);
                }
                else if(flightToEdit.Departure < DateTime.Now)
                {
                    ViewData["Error"] = "Date of departure should be after today";
                    return View(model);
                }
                try
                {
                    context.Update(flightToEdit);
                    context.SaveChanges();
                    return View("Flights", GetAllUpcomingFlights());
                }
                catch
                {
                    ViewData["Error"] = "Something went wrong";
                    return View(model);
                }
            }
            ViewData["Error"] = "Something went wrong";
            return View(model);
        }
    }
}