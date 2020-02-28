using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITCAir.Data.Entities;
using ITCAir.Web.Models.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ITCAir.Web.Controllers
{
    public class AccountsController : Controller
    {
        private readonly SignInManager<User> signInManager;

        public AccountsController(SignInManager<User> signInManager)
        {
            this.signInManager = signInManager;
        }

        public async Task<IActionResult> SignOut()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Reservations");
        }

        [HttpGet]
        public IActionResult SignIn()
        {
            ViewData["Message"] = "";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(UserSignInViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(
                    model.UserName, model.Password, model.RememberUser, false);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Reservations");
                }
            }

            ViewData["Message"] = "Invalid creditentials!";
            return View();
        }
    }
}