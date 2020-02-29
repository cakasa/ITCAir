using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITCAir.Web.GlobalConstants
{
    public static class ConfirmEmail
    {
        public static int ReservationIdGoing { get; set; }

        public static int ReservationIdReturning { get; set; }

        public static int ValidationToken { get; set; }

        public static bool EmailConfirmed { get; set; }
    }
}
