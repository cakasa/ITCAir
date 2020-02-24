using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ITCAir.Web.Models.User
{
    public class UserSignUpViewModel
    {
        [Required(ErrorMessage = "Enter a username.")]
        public string UserName { get; set; }

        [Display(Name = "Password")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[$@$!%*_?&])[A-Za-z\d$@_!%*?&]{6,}$",
            ErrorMessage = "Passwords must be at least 8 characters long and contain each of the following: [upper case character(A-Z)] , [lowercase character(a-z)] , [digit (0-9)] , [special character (e.g. !@_#$%^&*)]")]
        [Required(ErrorMessage = "Enter a password.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm your password.")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords don't match.")]
        public string ConfirmPassword { get; set; }


        [Required(ErrorMessage = "Enter an email address.")]
        [EmailAddress(ErrorMessage = "Enter a valid email address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Enter a first name.")]
        [MinLength(2, ErrorMessage = "First name must contain at least 2 letters.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Enter a last name.")]
        [MinLength(2, ErrorMessage = "Last name must contain at least 2 letters.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Enter an address.")]
        [MinLength(5, ErrorMessage = "Address must contain at least 5 letters.")]
        public string Address { get; set; }

        [Required(ErrorMessage ="Enter a phone number.")]
        [Phone(ErrorMessage ="Enter a valid phone number.")]
        public string Phone { get; set; }
    }
}
