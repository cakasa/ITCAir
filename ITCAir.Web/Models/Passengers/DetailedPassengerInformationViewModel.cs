using ITCAir.Data.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ITCAir.Web.Models.Passengers
{
    public class DetailedPassengerInformationViewModel
    {
        [Required]
        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public string PersonalId { get; set; }

        public string PhoneNumber { get; set; }

        public string Nationality { get; set; }

        public TicketType Ticket { get; set; }
    }
}
