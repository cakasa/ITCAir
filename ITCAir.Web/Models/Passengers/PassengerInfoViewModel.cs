using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ITCAir.Data.Enum;

namespace ITCAir.Web.Models.Passengers
{
    public class PassengerInfoViewModel
    {
        [Required]
        [MinLength(2, ErrorMessage = "From should be at least 2 symbols long")]
        public string FirstName { get; set; }
        [Required]
        [MinLength(2, ErrorMessage = "From should be at least 2 symbols long")]
        public string MiddleName { get; set; }
        [Required]
        [MinLength(2, ErrorMessage = "From should be at least 2 symbols long")]
        public string LastName { get; set; }
        [Required]
        public string Egn { get; set; }
        [Required]
        [Range(8,10)]
        public string PhoneNumber { get; set; }
        [Required]
        public string Nationality { get; set; }

        public TicketType Ticket  { get; set; }

        public string ConfirmEmail { get; set; }
    }
}
