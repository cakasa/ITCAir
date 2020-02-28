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
        public DateTime Departure { get; set; }
        public DateTime Arrival { get; set; }
        public string PlaneModel { get; set; }
        public string PlaneId { get; set; }
        public string PilotName { get; set; }
        public int EconomyCapacity { get; set; }
        public int BusinessCapacity { get; set; }

        public virtual ICollection<Reservation> Reservations { get; set; }
    }
}
