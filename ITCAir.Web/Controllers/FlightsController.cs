﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ITCAir.Web.Controllers
{
    public class FlightsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}