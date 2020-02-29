using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace ITCAir.Web.Attributes
{
    public class UpcomingDateAttribute : ValidationAttribute
    {
        private static readonly string ErrorMessage = $"Date can't be before {DateTime.Now.ToString("dd/MM/yyyy")}";

        public override bool IsValid(object value)
        {
            DateTime date = (DateTime)value;

            if (date < DateTime.Now)
            {
                return false;
            }
            return true;
        }
        protected override ValidationResult IsValid(object value,
           ValidationContext validationContext)
        {
            DateTime date = (DateTime)value;
            date = date.AddHours(23);
            date = date.AddMinutes(59);

            if (date < DateTime.Now)
            {
                return new ValidationResult(ErrorMessage);
            }
            return ValidationResult.Success;
        }
    }
}
