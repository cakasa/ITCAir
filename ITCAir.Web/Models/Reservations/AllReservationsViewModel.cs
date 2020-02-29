using ITCAir.Data.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITCAir.Web.Models.Reservations
{
    public class AllReservationsViewModel
    {
        public string Filter { get; set; }
        public ReservationFilterType FilterType { get; set; }

        public List<ReservationViewModel> AllReservations { get; set; }
    }
}
