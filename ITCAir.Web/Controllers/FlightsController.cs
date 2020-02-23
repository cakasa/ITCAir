using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ITCAir.Web.Controllers
{
    [Authorize(Roles = "Admin, User")]
    public class FlightsController : Controller
    {
        public IActionResult Index()
        {
            return View("Flights");
        }
    }
}