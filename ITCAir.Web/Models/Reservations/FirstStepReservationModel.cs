using ITCAir.Web.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ITCAir.Web.Models.Reservations
{
    public class FirstStepReservationModel
    {
        [Required]
        public string From { get; set; }

        [Required]
        public string To { get; set; }

        [Required]
        [DataType(DataType.Time, ErrorMessage = "Enter a valid time")]
        [UpcomingDate]
        public DateTime DepartureDate { get; set; }

        [Required]
        [DataType(DataType.Time, ErrorMessage = "Enter a valid time")]
        [UpcomingDate]
        public DateTime ReturnDate { get; set; }

        public bool OneWay { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Negative values or 0 are not accepted")]
        public int People { get; set; }
    }
}
