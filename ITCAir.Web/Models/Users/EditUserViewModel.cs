using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ITCAir.Web.Models.User
{
    public class EditUserViewModel
    {
        public string Username { get; set; }

        [Required(ErrorMessage = "Enter an email address.")]
        [EmailAddress(ErrorMessage = "Enter a valid email address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Enter a Personal ID")]
        [RegularExpression(@"^(\d){8,12}$",
            ErrorMessage = "Enter a valid Personal ID")]
        public string PersonalId { get; set; }

        [Required(ErrorMessage = "Enter a first name.")]
        [MinLength(2, ErrorMessage = "First name must contain at least 2 letters.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Enter a last name.")]
        [MinLength(2, ErrorMessage = "Last name must contain at least 2 letters.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Enter an address.")]
        [MinLength(5, ErrorMessage = "Address must contain at least 5 letters.")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Enter a phone number.")]
        [Phone(ErrorMessage = "Enter a valid phone number.")]
        public string Phone { get; set; }
    }
}
