// <copyright file="AccountController.cs" company="Felipe Pergher">
// Copyright (c) Felipe Pergher. All Rights Reserved.
// </copyright>

using System;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using BaseAdminProject.Areas.Identity.Pages.Account;
using BaseAdminProject.Business.Core;
using BaseAdminProject.Data.Models;
using BaseAdminProject.Models.FormModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace BaseAdminProject.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<BaseAdminUser> _userManager;
        private readonly SignInManager<BaseAdminUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<AccountController> _logger;

        public AccountController(UserManager<BaseAdminUser> userManager, SignInManager<BaseAdminUser> signInManager, IEmailSender emailSender, ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _logger = logger;
        }

        #region Anonimous

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Login()
        {
            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            return View(new LoginFormModel());
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(LoginFormModel loginForm, string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            if (ModelState.IsValid)
            {
                Microsoft.AspNetCore.Identity.SignInResult result;
                var user = await _userManager.FindByEmailAsync(loginForm.EmailUsername);
                if (user != null)
                {
                    result = await _signInManager.PasswordSignInAsync(user, loginForm.Password, loginForm.RememberMe, true);
                }
                else
                {
                    result = await _signInManager.PasswordSignInAsync(loginForm.EmailUsername, loginForm.Password, loginForm.RememberMe, true);
                }

                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");
                    return Redirect(returnUrl);
                }

                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    return RedirectToAction("Lockout");
                }

                ModelState.AddModelError(
                    string.Empty,
                    result.IsNotAllowed ? "Você precisa confirmar seu email para obter acesso ao sistema." : "Tentativa de login inválida.");
            }

            return View(loginForm);
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View(new ForgotPasswordFormModel());
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordFormModel forgotPasswordForm)
        {
            if (ModelState.IsValid)
            {
                BaseAdminUser user = await _userManager.FindByEmailAsync(forgotPasswordForm.Email);
                if (user != null && await _userManager.IsEmailConfirmedAsync(user))
                {
                    string code = await _userManager.GeneratePasswordResetTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                    string callbackUrl = Url.Action(
                        "ResetPassword",
                        "Account",
                        values: new { code },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(
                        forgotPasswordForm.Email,
                        "Redefinir Senha",
                        $"Por favor redefina sua senha <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicando aqui</a>.");
                }

                TempData[Globals.StatusMessageKey] = "Confira seu email para redefinir sua senha.";
                TempData[Globals.StatusMessageTypeKey] = Globals.StatusMessageTypeSuccess;

                return RedirectToAction(nameof(Login));
            }

            return View();
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult ResetPassword(string code = null)
        {
            if (code == null)
            {
                return NotFound();
            }

            var model = new ResetPasswordFormModel
            {
                Code = code,
            };
            return View(model);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordFormModel resetPasswordForm)
        {
            if (!ModelState.IsValid)
            {
                return View(resetPasswordForm);
            }

            BaseAdminUser user = await _userManager.FindByEmailAsync(resetPasswordForm.Email);
            if (user == null)
            {
                TempData[Globals.StatusMessageKey] = "Sua senha foi redefinida.";
                TempData[Globals.StatusMessageTypeKey] = Globals.StatusMessageTypeSuccess;

                return RedirectToAction(nameof(Login));
            }

            resetPasswordForm.Code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(resetPasswordForm.Code));
            IdentityResult result = await _userManager.ResetPasswordAsync(user, resetPasswordForm.Code, resetPasswordForm.Password);
            if (result.Succeeded)
            {
                var isLocked = await _userManager.IsLockedOutAsync(user);

                if (isLocked)
                {
                    await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.Now.AddDays(-1));
                }

                TempData[Globals.StatusMessageKey] = "Sua senha foi redefinida.";
                TempData[Globals.StatusMessageTypeKey] = Globals.StatusMessageTypeSuccess;

                return RedirectToAction(nameof(Login));
            }

            foreach (IdentityError error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(resetPasswordForm);
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult ResendConfirmEmail()
        {
            return View(new ResendConfirmEmailFormModel());
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> ResendConfirmEmail(ResendConfirmEmailFormModel resendConfirmEmailForm)
        {
            if (!ModelState.IsValid)
            {
                return View(resendConfirmEmailForm);
            }

            BaseAdminUser user = await _userManager.FindByEmailAsync(resendConfirmEmailForm.Email);
            if (user != null)
            {
                string userId = await _userManager.GetUserIdAsync(user);
                string code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                string callbackUrl = Url.Action(
                    "ConfirmEmail",
                    "Account",
                    values: new { userId, code },
                    protocol: Request.Scheme);

                await _emailSender.SendEmailAsync(
                    resendConfirmEmailForm.Email,
                    "Confirme seu email",
                    $"Por favor confirme sua conta <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicando aqui</a>.");
            }

            TempData[Globals.StatusMessageKey] = "Email de verificação enviado. Por favor confira seu email.!";
            TempData[Globals.StatusMessageTypeKey] = Globals.StatusMessageTypeSuccess;

            return RedirectToAction(nameof(Login));
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (!string.IsNullOrEmpty(userId) && !string.IsNullOrEmpty(code))
            {
                BaseAdminUser user = await _userManager.FindByIdAsync(userId);
                if (user != null)
                {
                    code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
                    IdentityResult result = await _userManager.ConfirmEmailAsync(user, code);

                    TempData[Globals.StatusMessageKey] = result.Succeeded ? "Obrigado por confirmar seu email." : "Erro ao confirmar seu email.";
                    TempData[Globals.StatusMessageTypeKey] = result.Succeeded ? Globals.StatusMessageTypeSuccess : Globals.StatusMessageTypeDanger;

                    return RedirectToAction(nameof(Login));
                }
            }

            TempData[Globals.StatusMessageKey] = "Erro ao confirmar seu email.";
            TempData[Globals.StatusMessageTypeKey] = Globals.StatusMessageTypeDanger;

            return RedirectToAction(nameof(Login));
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Lockout()
        {
            return View();
        }

        #endregion

        #region Manage

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Logout(string returnUrl = null)
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            if (returnUrl != null)
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction(nameof(Login));
        }

        #endregion
    }
}
