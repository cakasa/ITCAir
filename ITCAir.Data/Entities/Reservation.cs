using System.Collections;
using System.Collections.Generic;

namespace ITCAir.Data.Entities
{
    public class Reservation
    {
        public int Id { get; set; }

        public string Email { get; set; }

        public int FlightId { get; set; }

        public virtual Flight Flight { get; set; }

        public bool IsConfirmed { get; set; }

        public virtual ICollection<Passenger> Passengers { get; set; }  
    }
}