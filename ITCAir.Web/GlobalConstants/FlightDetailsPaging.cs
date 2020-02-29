using ITCAir.Web.Models.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITCAir.Web.GlobalConstants
{
    public static class FlightDetailsPaging
    {
        public static int FlightId { get; set; }
        public static PagerViewModel Pager { get; set; }

        public static void Clear()
        {
            Pager = new PagerViewModel();
            Pager.CurrentPage = 1;
            Pager.PagesCount = 1;
            Pager.PageSize = 5;
        }
    }
}
