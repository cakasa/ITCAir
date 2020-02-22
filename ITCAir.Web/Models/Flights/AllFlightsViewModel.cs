using ITCAir.Web.Models.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITCAir.Web.Models.Flights
{
    public class AllFlightsViewModel
    {
        public PagerViewModel PagerOnGoing { get; set; }

        public PagerViewModel PagerOnReturning { get; set; }

        public ICollection<FlightViewModel> GoingFlights { get; set; }

        public ICollection<FlightViewModel> ReturningFlights { get; set; }
    }
}
