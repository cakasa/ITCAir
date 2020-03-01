using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using ITCAir.Data;
using ITCAir.Data.Entities;
using ITCAir.Web.GlobalConstants;
using ITCAir.Web.Models.Passengers;
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

        public IActionResult SendEmail(PassengerInfoViewModel passengerInfo)
        {
            

            StringBuilder sb = FormatBody();

            MailMessage mail = new MailMessage();
            mail.To.Add(passengerInfo.ConfirmEmail);
            mail.From = new MailAddress(EmailFormating.EmailSender);
            mail.Subject = EmailFormating.EmailSubject;
            string Body = sb.ToString().Trim();
            mail.Body = Body;
            mail.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = EmailFormating.EmailHost;
            smtp.Port = 587;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new System.Net.NetworkCredential(EmailFormating.EmailSender, EmailFormating.EmailSenderPassworfd); // Enter seders User name and password  
            smtp.EnableSsl = true;
            smtp.Send(mail);

            SaveData(passengerInfo.ConfirmEmail);

            return View("ThanksForReservation");
        }

        public IActionResult ThanksForReservation()
        {
            return View("ThanksForReservation");
        }

        public void SaveData(string personEmail)
        {
            Reservation curReservationGoing = new Reservation();
            curReservationGoing.Email = personEmail;
            curReservationGoing.FlightId = FlightsClass.GoingPlaneId;

            this.context.Reservations.Add(curReservationGoing);
            this.context.SaveChanges();
            ConfirmEmail.ReservationIdGoing = this.context.Reservations.ToList().Last().Id;
            int curReservationId = this.context.Reservations.ToList().Last().Id;

            List<Passenger> allPassengers = new List<Passenger>();
            Passenger curPassanger = new Passenger();
            foreach (var passenger in ModelClass.Passengers)
            {
                curPassanger = new Passenger();
                curPassanger.FirstName = passenger.FirstName;
                curPassanger.MiddleName = passenger.MiddleName;
                curPassanger.LastName = passenger.LastName;
                curPassanger.PersonalId = passenger.Egn;
                curPassanger.Phone = passenger.PhoneNumber;
                curPassanger.Nationality = passenger.Nationality;
                if ((int)passenger.Ticket == 1)
                    curPassanger.IsBusinessClass = true;
                else
                    curPassanger.IsBusinessClass = false;
                curPassanger.ReservationId = curReservationId;
                this.context.Passengers.Add(curPassanger);
            }
                this.context.SaveChanges();

            if (!ModelClass.OneWay)
            {
                Reservation curReservationReturning = new Reservation();
                curReservationReturning.Email = personEmail;
                curReservationReturning.FlightId = FlightsClass.ReturningPlaneId;

                this.context.Reservations.Add(curReservationReturning);
                this.context.SaveChanges();
                ConfirmEmail.ReservationIdReturning = this.context.Reservations.ToList().Last().Id;
                int curReservationReturningId = this.context.Reservations.ToList().Last().Id;

                foreach (var passenger in ModelClass.Passengers)
                {
                    curPassanger = new Passenger();
                    curPassanger.FirstName = passenger.FirstName;
                    curPassanger.MiddleName = passenger.MiddleName;
                    curPassanger.LastName = passenger.LastName;
                    curPassanger.PersonalId = passenger.Egn;
                    curPassanger.Phone = passenger.PhoneNumber;
                    curPassanger.Nationality = passenger.Nationality;
                    if ((int)passenger.Ticket == 1)
                        curPassanger.IsBusinessClass = true;
                    else
                        curPassanger.IsBusinessClass = false;
                    curPassanger.ReservationId = curReservationReturningId;
                    this.context.Passengers.Add(curPassanger);
                }
                this.context.SaveChanges();

            }
        }

        private  StringBuilder FormatBody()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Passangers Registered: {EmailFormating.NewLine}");

            foreach (var passenger in ModelClass.Passengers)
            {
                sb.AppendLine($"First name:{passenger.FirstName} Last name:{passenger.LastName} Ticket type:{passenger.Ticket} {EmailFormating.NewLine}");
            }

            sb.AppendLine($"Going flight:{EmailFormating.NewLine}");
            var flightFrom = this.context.Flights.First(f => f.Id == FlightsClass.GoingPlaneId);
            sb.AppendLine($"Flight from:{flightFrom.From} To:{flightFrom.To} Depart at:{flightFrom.Departure.ToShortTimeString()}" +
                $" Arrive At:{flightFrom.Arrival.ToShortTimeString()} {EmailFormating.NewLine}");

            if (!ModelClass.OneWay)
            {
                sb.AppendLine($"Returning flight:{EmailFormating.NewLine}");
                var returningFlight = this.context.Flights.First(f => f.Id == FlightsClass.ReturningPlaneId);
                sb.AppendLine($"Flight from:{returningFlight.From} To:{returningFlight.To} Depart at:{returningFlight.Departure.ToShortTimeString()}" +
                $" Arrive at:{returningFlight.Arrival.ToShortTimeString()} {EmailFormating.NewLine}");
            }
            Random generator = new Random();
            int generatedToken = int.Parse(generator.Next(1, 999999).ToString("D6"));
            sb.AppendLine($"Your Confirmation code is: {EmailFormating.NewLine}");
            sb.AppendLine($"{generatedToken}");
            ConfirmEmail.ValidationToken = generatedToken;

            return sb;
        }
    }
}