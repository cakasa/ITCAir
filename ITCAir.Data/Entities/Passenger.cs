using System;
using System.Collections.Generic;
using System.Text;

namespace ITCAir.Data.Entities
{
    public class Passenger
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string PersonalId { get; set; }
        public string Phone { get; set; }
        public string Nationality { get; set; }
        public bool IsBusinessClass { get; set; }

        public int ReservationId { get; set; }
        public virtual Reservation Reservation { get; set; }
    }
}
