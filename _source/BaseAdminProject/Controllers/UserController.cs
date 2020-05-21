// <copyright file="UserController.cs" company="Felipe Pergher">
// Copyright (c) Felipe Pergher. All Rights Reserved.
// </copyright>

using BaseAdminProject.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BaseAdminProject.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<BaseAdminUser> _userManager;

        public UserController(UserManager<BaseAdminUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AddUser()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddUser(string a)
        {
            return View();
        }

        [HttpGet]
        public IActionResult EditUser(string id)
        {
            return View();
        }

        [HttpPost]
        public IActionResult EditUser()
        {
            return View();
        }

        [HttpPost]
        public IActionResult DeleteUser()
        {
            return Ok();
        }
    }
}