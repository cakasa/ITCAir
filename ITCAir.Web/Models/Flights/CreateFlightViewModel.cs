using ITCAir.Web.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ITCAir.Web.Models.Flights
{
    public class CreateFlightViewModel
    {
        [Required]
        [MinLength(2, ErrorMessage ="From should be at least 2 symbols long")]
        public string From { get; set; }
        [Required]
        [MinLength(2, ErrorMessage = "To should be at least 2 symbols long")]
        public string To { get; set; }

        [Required]
        [UpcomingDate]
        [DataType(DataType.Date, ErrorMessage = "Enter a valid date")]
        public DateTime DateOfDeparture { get; set; }

        [Required]
        [DataType(DataType.Time, ErrorMessage = "Enter a valid time")]
        public DateTime TimeOfDeparture { get; set; }


        [Required]
        [UpcomingDate]
        [DataType(DataType.Date, ErrorMessage = "Enter a valid date")]
        public DateTime DateOfArrival { get; set; }


        [Required]
        [DataType(DataType.Time, ErrorMessage = "Enter a valid time")]
        public DateTime TimeOfArrival { get; set; }


        [Required]
        [MinLength(2, ErrorMessage = "Airplane type should be at least 2 symbols long")]
        public string PlanetModel { get; set; }
        [Required]
        [MinLength(6, ErrorMessage = "Airplane Id should be at least 6 symbols long")]
        public string AirplaneId { get; set; }
        [Required]
        [MinLength(2, ErrorMessage = "Pilot name should be at least 2 symbols long")]
        public string PilotName { get; set; }
        [Required]
        [Range(0, 10000, ErrorMessage = "Economy seats should be non-negative")]
        public int EconomyCapacity { get; set; }
        [Required]
        [Range(0, 10000, ErrorMessage = "Business seats should be non-negative")]
        public int BusinessCapacity { get; set; }
    }
}
