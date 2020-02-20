using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ITCAir.Web.Controllers
{
    public class IndexController : Controller
    {
        // Zashto e IndexController??
        public IActionResult Index()
        {
            return View();
        }
    }
}