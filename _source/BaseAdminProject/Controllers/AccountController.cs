// <copyright file="AccountController.cs" company="Felipe Pergher">
// Copyright (c) Felipe Pergher. All Rights Reserved.
// </copyright>

using BaseAdminProject.Business.Core;
using BaseAdminProject.Data.Models;
using BaseAdminProject.Models.FormModels;
using BaseAdminProject.Models.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace BaseAdminProject.Controllers
{
    [Authorize]
    [AutoValidateAntiforgeryToken]
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
                string userEmailUpper = loginForm.EmailUsername.ToUpper();
                BaseAdminUser user = _userManager.Users
                                         .Include(x => x.UserInfo)
                                         .FirstOrDefault(x => x.NormalizedEmail == userEmailUpper || x.NormalizedUserName == userEmailUpper);

                if (user == null)
                {
                    ModelState.AddModelError(
                        string.Empty,
                        "Tentativa de login inválida.");

                    return View(loginForm);
                }

                // Todo test
                if (!user.UserInfo.Active)
                {
                    ModelState.AddModelError(
                        string.Empty,
                        "Conta desativada pelos administradores.");
                    return View(loginForm);
                }

                Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(user, loginForm.Password, loginForm.RememberMe, true);

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
                bool isLocked = await _userManager.IsLockedOutAsync(user);

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
                await SendConfirmationEmail(user, resendConfirmEmailForm.Email);
            }

            TempData[Globals.StatusMessageKey] = "Email de verificação enviado. Por favor confira seu email.!";
            TempData[Globals.StatusMessageTypeKey] = Globals.StatusMessageTypeSuccess;

            return RedirectToAction(nameof(Login));
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId, string code, string redirectUrl)
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

                    if (!string.IsNullOrEmpty(redirectUrl))
                    {
                        return Redirect(redirectUrl);
                    }

                    return RedirectToAction(nameof(Login));
                }
            }

            TempData[Globals.StatusMessageKey] = "Erro ao confirmar seu email.";
            TempData[Globals.StatusMessageTypeKey] = Globals.StatusMessageTypeDanger;

            return RedirectToAction(nameof(Login));
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> ConfirmEmailChange(string userId, string email, string code, string redirectUrl)
        {
            if (userId == null || email == null || code == null)
            {
                TempData[Globals.StatusMessageKey] = "Erro ao trocar o email.";
                TempData[Globals.StatusMessageTypeKey] = Globals.StatusMessageTypeDanger;
                return RedirectToAction(nameof(Login));
            }

            BaseAdminUser user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            IdentityResult result = await _userManager.ChangeEmailAsync(user, email, code);
            if (!result.Succeeded)
            {
                TempData[Globals.StatusMessageKey] = "Erro ao trocar o email.";
                TempData[Globals.StatusMessageTypeKey] = Globals.StatusMessageTypeDanger;
                return RedirectToAction(nameof(Login));
            }

            await _signInManager.RefreshSignInAsync(user);

            TempData[Globals.StatusMessageKey] = "Obrigado por confirmar sua troca de email.";
            TempData[Globals.StatusMessageTypeKey] = Globals.StatusMessageTypeSuccess;

            if (!string.IsNullOrEmpty(redirectUrl))
            {
                return Redirect(redirectUrl);
            }

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

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            BaseAdminUser user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return BadRequest();
            }

            string userName = await _userManager.GetUserNameAsync(user);
            string userId = await _userManager.GetUserIdAsync(user);
            string phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            string email = await _userManager.GetEmailAsync(user);

            var profileForm = new ProfileViewModel
            {
                PhoneForm = new PhoneFormModel
                {
                    PhoneNumber = phoneNumber
                },
                UsernameForm = new UsernameFormModel
                {
                    UserId = userId,
                    UserName = userName
                },
                EmailForm = new EmailFormModel
                {
                    UserId = userId,
                    Email = email,
                    IsConfirmed = await _userManager.IsEmailConfirmedAsync(user)
                }
            };

            return View(profileForm);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUsername(UsernameFormModel usernameForm)
        {
            if (ModelState.IsValid)
            {
                BaseAdminUser user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return BadRequest("Alguma coisa deu errado!");
                }

                string userName = await _userManager.GetUserNameAsync(user);

                if (usernameForm.UserName != userName)
                {
                    IdentityResult setUsernameResult = await _userManager.SetUserNameAsync(user, usernameForm.UserName);
                    if (!setUsernameResult.Succeeded)
                    {
                        return BadRequest("Alguma coisa deu errado salvando o usuário!");
                    }

                    await _signInManager.RefreshSignInAsync(user);

                    return Ok(new { message = "Seu usuário foi atualizado!", type = Globals.StatusMessageTypeSuccess });
                }

                return Ok(new { message = "Seu usuário não foi alterado!", type = Globals.StatusMessageTypeInfo });
            }

            return PartialView("Partials/UsernameForm", usernameForm);
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePhone(PhoneFormModel phoneForm)
        {
            if (ModelState.IsValid)
            {
                BaseAdminUser user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return BadRequest("Alguma coisa deu errado!");
                }

                string phoneNumber = await _userManager.GetPhoneNumberAsync(user);
                if (phoneForm.PhoneNumber != phoneNumber)
                {
                    IdentityResult setPhoneResult = await _userManager.SetPhoneNumberAsync(user, phoneForm.PhoneNumber);
                    if (!setPhoneResult.Succeeded)
                    {
                        return BadRequest("Alguma coisa deu errado salvando o telefone!");
                    }

                    await _signInManager.RefreshSignInAsync(user);

                    return Ok(new { message = "Seu número de telefone foi atualizado!", type = Globals.StatusMessageTypeSuccess });
                }

                return Ok(new { message = "Seu número de telefone não foi alterado!", type = Globals.StatusMessageTypeInfo });
            }

            return PartialView("Partials/PhoneForm", phoneForm);
        }

        [HttpPost]
        public async Task<IActionResult> SendVerificationEmail()
        {
            BaseAdminUser user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            string email = await _userManager.GetEmailAsync(user);
            await SendConfirmationEmail(user, email, Url.Action("Index", "Account"));

            return Ok(new { message = "Email de verificação enviado. Por favor confira seu email.", type = Globals.StatusMessageTypeInfo });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateEmail(EmailFormModel emailForm)
        {
            if (ModelState.IsValid)
            {
                BaseAdminUser user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return BadRequest("Alguma coisa deu errado!");
                }

                string email = await _userManager.GetEmailAsync(user);
                if (emailForm.Email != email)
                {
                    string userId = await _userManager.GetUserIdAsync(user);
                    string code = await _userManager.GenerateChangeEmailTokenAsync(user, emailForm.Email);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                    string callbackUrl = Url.Action(
                        "ConfirmEmailChange",
                        "Account",
                        values: new { userId, email = emailForm.Email, code, redirectUrl = Url.Action("Index", "Account") },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(
                        emailForm.Email,
                        "Confirme seu email",
                        $"Por favor confirme sua conta <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicando aqui</a>.");

                    return Ok(new { message = "Link de confirmação de troca de email enviado. Por favor verifique seu email.", type = Globals.StatusMessageTypeSuccess });
                }

                return Ok(new { message = "Seu email não foi alterado!", type = Globals.StatusMessageTypeInfo });
            }

            return PartialView("Partials/EmailForm", emailForm);
        }

        [HttpGet]
        public IActionResult ChangePassword()
        {
            var changePasswordForm = new ChangePasswordFormModel();
            return View(changePasswordForm);
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordFormModel changePasswordForm)
        {
            if (ModelState.IsValid)
            {
                BaseAdminUser user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return NotFound();
                }

                IdentityResult changePasswordResult = await _userManager.ChangePasswordAsync(user, changePasswordForm.OldPassword, changePasswordForm.NewPassword);
                if (!changePasswordResult.Succeeded)
                {
                    foreach (IdentityError error in changePasswordResult.Errors)
                    {
                        string description = error.Description == "Incorrect password." ? "Senha incorreta." : error.Description;
                        ModelState.AddModelError(string.Empty, description);
                    }

                    return View(changePasswordForm);
                }

                await _signInManager.RefreshSignInAsync(user);
                _logger.LogInformation("User changed their password successfully.");

                TempData[Globals.StatusMessageKey] = "Sua senha foi atualizada com sucesso!";
                TempData[Globals.StatusMessageTypeKey] = Globals.StatusMessageTypeSuccess;

                return RedirectToAction(nameof(ChangePassword));
            }

            return View(changePasswordForm);
        }

        [HttpGet]
        public async Task<IActionResult> PersonalInfo()
        {
            BaseAdminUser user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return BadRequest();
            }

            var userInfo = _userManager.Users.Include(x => x.UserInfo).FirstOrDefault(x => x.Id == user.Id)?.UserInfo;

            if (userInfo == null)
            {
                return BadRequest();
            }

            var personalInfoForm = new PersonalInfoFormModel
            {
                Name = userInfo.Name,
                BirthdayDate = userInfo.BirthdayDate.ToShortDateString()
            };

            return View(personalInfoForm);
        }

        [HttpPost]
        public async Task<IActionResult> PersonalInfo(PersonalInfoFormModel personalInfoForm)
        {
            if (ModelState.IsValid)
            {
                BaseAdminUser user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return NotFound();
                }

                var userWithInfo = _userManager.Users.Include(x => x.UserInfo).FirstOrDefault(x => x.Id == user.Id);

                if (userWithInfo?.UserInfo == null)
                {
                    return NotFound();
                }

                userWithInfo.UserInfo.Name = personalInfoForm.Name;
                userWithInfo.UserInfo.BirthdayDate = DateTime.Parse(personalInfoForm.BirthdayDate);

                var result = await _userManager.UpdateAsync(userWithInfo);

                if (result.Succeeded)
                {
                    TempData[Globals.StatusMessageKey] = "Sua informações foram atualizadas com sucesso!";
                    TempData[Globals.StatusMessageTypeKey] = Globals.StatusMessageTypeSuccess;

                    return RedirectToAction(nameof(PersonalInfo));
                }

                TempData[Globals.StatusMessageKey] = "Sua informações não foram atualizadas!";
                TempData[Globals.StatusMessageTypeKey] = Globals.StatusMessageTypeDanger;
            }

            return View(personalInfoForm);
        }

        [HttpGet]
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

        #region Private Methods

        private async Task SendConfirmationEmail(BaseAdminUser user, string email, string redirectUrl = "/")
        {
            string userId = await _userManager.GetUserIdAsync(user);
            string code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            string callbackUrl = Url.Action(
                "ConfirmEmail",
                "Account",
                values: new { userId, code, redirectUrl },
                protocol: Request.Scheme);

            await _emailSender.SendEmailAsync(
                email,
                "Confirme seu email",
                $"Por favor confirme sua conta <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicando aqui</a>.");
        }

        #endregion
    }
}
