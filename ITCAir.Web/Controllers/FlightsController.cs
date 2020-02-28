using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITCAir.Data;
using ITCAir.Data.Entities;
using ITCAir.Web.Models.Flights;
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

        public AllFlightsViewModel GetAllUpcomingFlights()
        {
            List<Flight> allFlights = context.Flights.Where(x => x.DepartureTime > DateTime.Now).ToList();
            List<SingleFlightViewModel> flightsWithAppropriateData = new List<SingleFlightViewModel>();
            foreach (var singleFlight in allFlights)
            {
                SingleFlightViewModel flight = new SingleFlightViewModel()
                {
                    Id = singleFlight.Id,
                    From = singleFlight.From,
                    To = singleFlight.To,
                    Departure = singleFlight.DepartureTime,
                    Duration = singleFlight.Arrival - singleFlight.DepartureTime
                };
                flightsWithAppropriateData.Add(flight);
            }
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
                    CapacityBusiness = model.BusinessCapacity,
                    CapacityEconomy = model.EconomyCapacity,
                    PilotName = model.PilotName,
                    PlaneModel = model.PlanetModel,
                    PlaneId = model.AirplaneId,
                    Reservations = new List<Reservation>(),
                    Arrival = new DateTime(model.DateOfArrival.Year,
                                           model.DateOfArrival.Month,
                                           model.DateOfArrival.Day,
                                           model.TimeOfArrival.Hour,
                                           model.TimeOfArrival.Minute,
                                           0),
                    DepartureTime = new DateTime(model.DateOfDeparture.Year,
                                                 model.DateOfDeparture.Month,
                                                 model.DateOfDeparture.Day,
                                                 model.TimeOfDeparture.Hour,
                                                 model.TimeOfDeparture.Minute,
                                                 0)
                };

                if(flight.Arrival < flight.DepartureTime)
                {
                    ViewData["Error"] = "Date of Arrival cannot be before Date of Departure!";
                    return View("CreateFlight");
                }
                context.Flights.Add(flight);
                context.SaveChanges();
            }
            return View("Flights", GetAllUpcomingFlights());
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("{id}")]
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

        public IActionResult FlightDetails(int? id)
        {
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult EditFlight(int? id)
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles ="Admin")]
        public IActionResult EditFlight(EditFlightViewModel flight)
        {
            return View();
        }
    }
}