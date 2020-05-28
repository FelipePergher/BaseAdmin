// <copyright file="ForgotPassword.cshtml.cs" company="Felipe Pergher">
// Copyright (c) Felipe Pergher. All Rights Reserved.
// </copyright>

using BaseAdminProject.Business.Core;
using BaseAdminProject.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace BaseAdminProject.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ForgotPasswordModel : PageModel
    {
        private readonly UserManager<BaseAdminUser> _userManager;
        private readonly IEmailSender _emailSender;

        public ForgotPasswordModel(UserManager<BaseAdminUser> userManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = Globals.RequiredMessage)]
            [EmailAddress(ErrorMessage = Globals.EmailRequiredMessage)]
            public string Email { get; set; }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                BaseAdminUser user = await _userManager.FindByEmailAsync(Input.Email);
                if (user != null && await _userManager.IsEmailConfirmedAsync(user))
                {
                    string code = await _userManager.GeneratePasswordResetTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    string callbackUrl = Url.Page(
                        "/Account/ResetPassword",
                        pageHandler: null,
                        values: new { area = "Identity", code },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(
                        Input.Email,
                        "Redefinir Senha",
                        $"Por favor redefina sua senha <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicando aqui</a>.");
                }

                TempData[Globals.StatusMessageKey] = "Confira seu email para redefinir sua senha.";
                TempData[Globals.StatusMessageTypeKey] = Globals.StatusMessageTypeSuccess;

                return RedirectToPage("./Login");
            }

            return Page();
        }
    }
}
