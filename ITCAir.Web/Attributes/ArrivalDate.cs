using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ITCAir.Web.Attributes
{
    public class ArrivalDate : ValidationAttribute
    {
        private readonly DateTime departureDate;
        private readonly string ErrorMessage = "Date of arrival should be after date of departure!";
        public ArrivalDate(DateTime departureDate)
        {
            this.departureDate = departureDate;
        }

        protected override ValidationResult IsValid(object value,
           ValidationContext validationContext)
        {
            DateTime arrivalDate = (DateTime)value;

            if(arrivalDate > departureDate)
            {
                return ValidationResult.Success;
            }
            return new ValidationResult(ErrorMessage);
        }
    }
}
