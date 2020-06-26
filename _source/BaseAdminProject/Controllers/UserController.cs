// <copyright file="UserController.cs" company="Felipe Pergher">
// Copyright (c) Felipe Pergher. All Rights Reserved.
// </copyright>

using BaseAdminProject.Business.Core;
using BaseAdminProject.Data.Models;
using BaseAdminProject.Models.FormModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace BaseAdminProject.Controllers
{
    [Authorize(Roles = Roles.Admin)]
    [AutoValidateAntiforgeryToken]
    public class UserController : Controller
    {
        private readonly UserManager<BaseAdminUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<UserController> _logger;

        public UserController(UserManager<BaseAdminUser> userManager, RoleManager<IdentityRole> roleManager, IEmailSender emailSender, ILogger<UserController> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _emailSender = emailSender;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AddUser()
        {
            var addUser = new AddUserFormModel();
            return View(addUser);
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(AddUserFormModel addUser)
        {
            if (ModelState.IsValid)
            {
                var user = new BaseAdminUser
                {
                    UserName = addUser.UserName,
                    Email = addUser.Email,
                    PhoneNumber = addUser.PhoneNumber,
                    UserInfo = new UserInfo
                    {
                        Active = addUser.Active,
                        Name = addUser.Name,
                        BirthdayDate = DateTime.Parse(addUser.BirthdayDate)
                    }
                };

                IdentityResult result = await _userManager.CreateAsync(user, addUser.Password);

                if (result.Succeeded)
                {
                    IdentityRole applicationRole = await _roleManager.FindByNameAsync(addUser.Role);
                    if (applicationRole != null)
                    {
                        await _userManager.AddToRoleAsync(user, applicationRole.Name);
                    }

                    string userId = await _userManager.GetUserIdAsync(user);
                    string code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                    string callbackUrl = Url.Action(
                        "ConfirmEmail",
                        "Account",
                        values: new { userId, code, redirectUrl = Url.Action("Index", "Home") },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(
                        user.Email,
                        "Confirme seu email",
                        $"Por favor confirme sua conta <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicando aqui</a>.");

                    TempData[Globals.StatusMessageKey] = "Usuário adicionado com sucesso!";
                    TempData[Globals.StatusMessageTypeKey] = Globals.StatusMessageTypeSuccess;
                    return RedirectToAction(nameof(Index));
                }

                _logger.LogError(result.Errors.FirstOrDefault()?.Description);
                this.ModelState.AddModelError(string.Empty, "Alguma coisa deu errado, tente novamente.");
            }

            return View(addUser);
        }
    }
}