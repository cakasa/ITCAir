using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ITCAir.Web.Models.Reservations
{
    public class EmailConfirmViewModel
    {
        [Required]
        public int EnteredNumber { get; set; }
    }
}
