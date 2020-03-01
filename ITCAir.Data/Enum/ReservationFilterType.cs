using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ITCAir.Data.Enum
{
    public enum ReservationFilterType
    {
        [Display(Name = "No Filter")]
        None,
        Email
    }
}
