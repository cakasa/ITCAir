using ITCAir.Data.Enum;
using ITCAir.Web.Models.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITCAir.Web.GlobalConstants
{
    public class ReservationsFilteringAndPaging
    {
        public static PagerViewModel Pager { get; set; }

        public static ReservationFilterType FilterType { get; set; }
        public static string Filter { get; set; }

        public static void ClearFilter()
        {
            FilterType = ReservationFilterType.None;
            Filter = null;
        }

        public static void Clear()
        {
            Pager = new PagerViewModel();
            Pager.CurrentPage = 1;
            Pager.PagesCount = 1;
            Pager.PageSize = 10;
            ClearFilter();
        }
    }
}
