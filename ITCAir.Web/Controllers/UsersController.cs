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
                // Do something
                return View("Index");
            }
            else
            {
                ViewData["Message"] = "Password or username is incorrect!";
                return View();
            }
        }

        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SignUp(UserSignUpViewModel user)
        {
            return View("SignIn");
        }
    }
}