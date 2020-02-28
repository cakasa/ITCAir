using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using ITCAir.Data;
using ITCAir.Web.GlobalConstants;
using ITCAir.Web.Models.Passanger;
using Microsoft.AspNetCore.Mvc;

namespace ITCAir.Web.Controllers
{
    public class PassengersController : Controller
    {
        private readonly ITCAirContext context;

        public PassengersController(ITCAirContext context)
        {
            this.context = context;
        }

        public IActionResult ProcessPassengerInfo(PassengerInfoViewModel passengerInfo)
        {
            ModelClass.Passengers.Add(passengerInfo);
            ModelClass.PassengersCount -= 1;
            return RedirectToAction("RegisterPassengers", "Reservations");
        }

        public IActionResult SendEmail(string email)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var passenger in ModelClass.Passengers)
            {
                sb.AppendLine($"{passenger.FirstName} {passenger.LastName}");
            }

            MailMessage mail = new MailMessage();
            mail.To.Add("cakas@abv.bg");
            mail.From = new MailAddress("gotinpet22@gmail.com");
            mail.Subject = "ConfirmByEmail";
            string Body = sb.ToString().Trim() ;
            mail.Body = Body;
            mail.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new System.Net.NetworkCredential("itcairsender@gmail.com", "DanchoiBojidar"); // Enter seders User name and password  
            smtp.EnableSsl = true;
            smtp.Send(mail);

            return View();
        }
    }
}