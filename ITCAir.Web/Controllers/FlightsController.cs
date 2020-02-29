using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITCAir.Data;
using ITCAir.Data.Entities;
using ITCAir.Data.Enum;
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
            return View(GetAllUpcomingFlights());
        }

        public IActionResult FlightsArchive()
        {
            return View(GetFlightsArchive());
        }

        public AllFlightsViewModel GetFlightsArchive()
        {
            List<Flight> allFlights = context.Flights.Where(x => x.Departure <= DateTime.Now).ToList();
            List<SingleFlightViewModel> flightsWithAppropriateData = new List<SingleFlightViewModel>();
            foreach (var singleFlight in allFlights)
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

            return model;
        }

        public AllFlightsViewModel GetAllUpcomingFlights()
        {
            List<Flight> allFlights = context.Flights.Where(x => x.Departure > DateTime.Now).ToList();
            List<SingleFlightViewModel> flightsWithAppropriateData = new List<SingleFlightViewModel>();
            foreach (var singleFlight in allFlights)
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
            if(ModelState.IsValid)
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
                    return View("CreateFlight");
                }
                else
                {
                    context.Flights.Add(flight);
                    context.SaveChanges();
                }
            }
            return View("Flights", GetAllUpcomingFlights());
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

        [HttpGet]
        public IActionResult FlightDetails(int? id)
        {
            if (context.Flights.Any(x => x.Id == id))
            {
                var flight = context.Flights.Find(id);

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

                var reservations = context.Reservations.Where(x => x.FlightId == id).ToList();

                var reservationsModel = new List<DetailedFlightReservationViewModel>();
                foreach(var reservation in reservations)
                {
                    var reservationModel = new DetailedFlightReservationViewModel();
                    reservationModel.Email = reservation.Email;

                    var passengers = context.Passengers.Where(x => x.ReservationId == reservation.Id);
                    var passengersModel = new List<DetailedPassengerInformationViewModel>();
                    foreach(var passenger in passengers)
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