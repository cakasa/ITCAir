using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITCAir.Data;
using ITCAir.Data.Entities;
using ITCAir.Web.Models;
using ITCAir.Web.Models.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ITCAir.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private UserManager<User> userManager;

        public UsersController(UserManager<User> userManager)
        {
            this.userManager = userManager;
        }

        [HttpGet]
        public IActionResult CreateUser()
        {
            ViewData["Unique"] = string.Empty;
            return View("CreateUser");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateUser(UserSignUpViewModel user)
        {
            User newUser = new User()
            {
                UserName = user.UserName,
                Address = user.Address,
                PhoneNumber = user.Phone,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName
            };

            if (userManager.Users.Any(u => u.Email == newUser.Email))
            {
                ViewData["Unique"] = "A user with this email already exists.";
                return View("CreateUser");
            }
            if (userManager.Users.Any(u => u.UserName == newUser.UserName))
            {
                ViewData["Unique"] = "A user with this username already exists.";
                return View("CreateUser");
            }

            var result = userManager.CreateAsync(newUser, user.Password).GetAwaiter().GetResult();
            if (result.Succeeded)
            {
                userManager.AddToRoleAsync(newUser, "User");
            }
            else
            {
                ViewData["Unique"] = "Something went wrong.";
                return View("CreateUser");
            }

            return View(GetUsers());
        }

        [HttpGet]
        public IActionResult Users()
        {
            return View(GetUsers());
        }

        public AllUsersViewModel GetUsers()
        {
            AllUsersViewModel model = new AllUsersViewModel();
            model.AllUsers = new List<UserInfoViewModel>();
            foreach (var user in userManager.Users)
            {
                UserInfoViewModel userInfo = new UserInfoViewModel()
                {
                    Email = user.Email,
                    Name = user.FirstName + " " + user.LastName,
                    Username = user.UserName
                };
                model.AllUsers.Add(userInfo);
            }

            return model;
        }
    }
}