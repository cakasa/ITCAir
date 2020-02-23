using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITCAir.Data;
using ITCAir.Web.Models;
using ITCAir.Web.Models.User;
using Microsoft.AspNetCore.Mvc;

namespace ITCAir.Web.Controllers
{
    public class UsersController : Controller
    {
        private readonly ITCAirContext context;

        public UsersController()
        {
            context = new ITCAirContext();
        }

        [HttpGet]
        public IActionResult SignIn()
        {
            ViewData["Message"] = "";
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SignIn(UserSignInViewModel user)
        {
            if (context.Users.Any(x => x.Username == user.Username && x.Password == user.Password))
            { 
                return View("Index");
            }
            else
            {
                ViewData["Message"] = "Invalid creditentials!";
                return View();
            }
        }

        [HttpGet]
        public IActionResult CreateUser()
        {
            return View("CreateUser");
        }

        [HttpPost]
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