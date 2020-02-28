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
        private int PageSize = 10;

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
        public IActionResult CreateUser(UserCreateViewModel user)
        {
            if (ModelState.IsValid)
            {
                User newUser = new User()
                {
                    UserName = user.UserName,
                    Address = user.Address,
                    PhoneNumber = user.Phone,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    PersonalId = user.PersonalId
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
            }
            else
            {
                ViewData["Unique"] = "Something went wrong.";
                return View("CreateUser");
            }
            return View("Users", GetUsers());
        }

        [HttpGet]
        public IActionResult Users()
        {
            AllUsersViewModel model = GetUsers();
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
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Username = user.UserName
                };
                model.AllUsers.Add(userInfo);
            }

            return model;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditUser(EditUserViewModel user)
        {
            try
            {
                User userToUpdate = userManager.FindByNameAsync(user.Username).GetAwaiter().GetResult();
                userToUpdate.FirstName = user.FirstName;
                userToUpdate.LastName = user.LastName;
                userToUpdate.PhoneNumber = user.Phone;
                userToUpdate.Email = user.Email;
                userToUpdate.Address = user.Address;
                userToUpdate.PersonalId = user.PersonalId;
                userManager.UpdateAsync(userToUpdate).GetAwaiter().GetResult();
            }
            catch
            {
                ViewData["Unique"] = "Something went wrong.";
                return View("EditUser");
            }

            ViewData["Unique"] = string.Empty;
            return View("Users", GetUsers());
        }

        [HttpGet("{username}")]
        public IActionResult EditUser(string username)
        {
            User user = userManager.FindByNameAsync(username).GetAwaiter().GetResult();
            EditUserViewModel userViewModel = new EditUserViewModel()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Username = user.UserName,
                PersonalId = user.PersonalId,
                Phone = user.PhoneNumber,
                Address = user.Address
            };

            ViewData["Unique"] = string.Empty;
            return View(userViewModel);
        }

        public IActionResult DeleteUser(string username)
        {
            User user = userManager.FindByNameAsync(username).GetAwaiter().GetResult();
            userManager.DeleteAsync(user).GetAwaiter().GetResult();

            return View("Users", GetUsers());
        }

        public IActionResult UserDetails(string username)
        {
            User user = userManager.FindByNameAsync(username).GetAwaiter().GetResult();
            UserDetailsViewModel userViewModel = new UserDetailsViewModel()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                Email = user.Email,
                PersonalId = user.PersonalId,
                Phone = user.PhoneNumber,
                Address = user.Address
            };

            return View(userViewModel);
        }
    }
}