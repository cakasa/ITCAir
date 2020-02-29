﻿using System;
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
        public string FirstName { get; set; }
        [Required]
        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public string Egn { get; set; }

        public string PhoneNumber { get; set; }

        public string Nationality { get; set; }

        public TicketType Ticket  { get; set; }

        public string ConfirmEmail { get; set; }
    }
}
