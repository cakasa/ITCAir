using ITCAir.Web.Models.Passengers;
using ITCAir.Web.Models.Reservations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITCAir.Web.GlobalConstants
{
    public static class ModelClass
    {
        static ModelClass()
        {
            Passengers = new List<PassengerInfoViewModel>();
        }
        public static ICollection<PassengerInfoViewModel> Passengers { get; set; }

        public static FirstStepReservationModel SecondModel { get; set; }

        public static bool OneWay { get; set; }

        public static string Test { get; set; }

        public static int PassengersCount { get; set; }
    }
}
