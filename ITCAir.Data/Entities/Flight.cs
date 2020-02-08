using System;
using System.Collections.Generic;
using System.Text;

namespace ITCAir.Data.Entities
{
    public class Flight
    {
        public int Id { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime Arrival { get; set; }
        public string PlaneId { get; set; }
        public string PilotName { get; set; }
        public int CapacityEconomy { get; set; }
        public int CapacityBusiness { get; set; }

        public virtual ICollection<Reservation> Reservations { get; set; }
    }
}
