using ITCAir.Data.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITCAir.Web.Models.Flights
{
    public class AllFlightsViewModel
    {
        public string Filter { get; set; }
        public FlightFilterType FilterType { get; set; }
        public List<SingleFlightViewModel> allFlights { get; set; }
    }
}
