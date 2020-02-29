using ITCAir.Web.Models.Reservations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITCAir.Web.Models.Flights
{
    public class FlightDetailsWithPassengersViewModel
    {
        public string From { get; set; }
        public string To { get; set; }
        public DateTime Departure { get; set; }
        public DateTime Arrival { get; set; }
        public string PlaneId { get; set; }

        public string PlaneModel { get; set; }
        public string PilotName { get; set; }
        public int CapacityEconomy { get; set; }
        public int CapacityBusiness { get; set; }

        public List<DetailedFlightReservationViewModel> Reservations { get; set; }
    }
}
