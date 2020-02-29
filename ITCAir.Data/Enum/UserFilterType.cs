using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ITCAir.Data.Enum
{
    public enum UserFilterType
    {
        [Display(Name = "No Filter")]
        None,
        [Display(Name = "First Name")]
        FirstName,
        [Display(Name = "Last Name")]
        LastName,
        [Display(Name = "Username")]
        Username,
        [Display(Name = "E-mail")]
        Email
    };
}
