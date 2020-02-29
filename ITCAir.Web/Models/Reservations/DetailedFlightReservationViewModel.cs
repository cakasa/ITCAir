using ITCAir.Web.Models.Passengers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITCAir.Web.Models.Reservations
{
    public class DetailedFlightReservationViewModel
    {
        public string Email { get; set; }
        public List<DetailedPassengerInformationViewModel> Passengers { get; set; } 
    }
}
