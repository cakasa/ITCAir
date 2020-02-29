using ITCAir.Data.Entities;
using ITCAir.Data.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITCAir.Web.Models.Reservations
{
    public class ReservationViewModel
    {
        public int ReservationId { get; set; }

        public Flight ReservedFlight { get; set; }

        public string ReservationEmail { get; set; }

        public ICollection<Passenger> AllPassengers { get; set; }
    }
}
