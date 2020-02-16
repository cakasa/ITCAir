using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ITCAir.Web.Attributes
{
    public class ReservationDateAttribute : ValidationAttribute
    {
        private static readonly int Day = DateTime.Now.Day;
        private static readonly int Month = DateTime.Now.Month;
        private static readonly int Year = DateTime.Now.Year;
        private static readonly string ErrorMessage = $"Reservation can't be before {DateTime.Now.ToString("dd/MM/yyyy")}";

        protected override ValidationResult IsValid(object value,
           ValidationContext validationContext)
    {
            int modelDay = (int)DateTime.Parse(value.ToString()).Day;
            int modelMonth = (int)DateTime.Parse(value.ToString()).Month;
            int modelYear = (int)DateTime.Parse(value.ToString()).Year;

            if (modelDay < Day && modelMonth < Month || modelYear < Year)
            {
                return new ValidationResult(ErrorMessage);
            }
            return ValidationResult.Success;
        }
    }
}
