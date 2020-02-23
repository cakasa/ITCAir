using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITCAir.Data;
using ITCAir.Web.Models;
using ITCAir.Web.Models.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ITCAir.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly ITCAirContext context;

        public UsersController(ITCAirContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public IActionResult CreateUser()
        {
            return View("CreateUser");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateUser(UserSignUpViewModel user)
        {
            return View("Users");
        }

        [HttpGet]
        public IActionResult Users()
        {
            return View();
        }
    }
}