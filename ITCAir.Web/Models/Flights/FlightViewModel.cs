using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITCAir.Web.Models.Flights
{
    public class FlightViewModel
    {
        public int Id { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public DateTime Departure { get; set; }
        public DateTime Arrival { get; set; }
    }
}
