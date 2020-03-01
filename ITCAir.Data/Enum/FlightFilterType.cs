using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ITCAir.Data.Enum
{
    public enum FlightFilterType
    {
        [Display(Name = "No Filter")]
        None,
        From,
        To
    }
}
