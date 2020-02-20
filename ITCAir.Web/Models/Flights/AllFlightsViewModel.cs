﻿using ITCAir.Web.Models.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITCAir.Web.Models.Flights
{
    public class AllFlightsViewModel
    {
        public PagerViewModel Pager { get; set; }

        public ICollection<FlightViewModel> Items { get; set; }
    }
}