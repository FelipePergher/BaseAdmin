// <copyright file="ResetPassword.cshtml.cs" company="Felipe Pergher">
// Copyright (c) Felipe Pergher. All Rights Reserved.
// </copyright>

using BaseAdminProject.Business.Core;
using BaseAdminProject.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;

namespace BaseAdminProject.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ResetPasswordModel : PageModel
    {
        private readonly UserManager<BaseAdminUser> _userManager;

        public ResetPasswordModel(UserManager<BaseAdminUser> userManager)
        {
            _userManager = userManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Display(Name = "Email")]
            [Required(ErrorMessage = Globals.RequiredMessage)]
            [EmailAddress(ErrorMessage = Globals.EmailRequiredMessage)]
            [DataType(DataType.EmailAddress)]
            public string Email { get; set; }

            [Display(Name = "Senha")]
            [Required(ErrorMessage = Globals.RequiredMessage)]
            [PasswordPropertyText]
            [DataType(DataType.Password)]
            [RegularExpression(@"^(?=.*[a-z])(?=.*\d).{8,}$", ErrorMessage = "A senha deve conter letras, números e minimo de 8 caracteres")]
            public string Password { get; set; }

            [Display(Name = "Confirmação de senha")]
            [Required(ErrorMessage = Globals.RequiredMessage)]
            [Compare(nameof(Password), ErrorMessage = "As senhas não conferem.")]
            [PasswordPropertyText]
            [DataType(DataType.Password)]
            public string ConfirmPassword { get; set; }

            [HiddenInput]
            public string Code { get; set; }
        }

        public IActionResult OnGet(string code = null)
        {
            if (code == null)
            {
                return NotFound();
            }

            Input = new InputModel
            {
                Code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code))
            };
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            BaseAdminUser user = await _userManager.FindByEmailAsync(Input.Email);
            if (user == null)
            {
                TempData[Globals.StatusMessageKey] = "Sua senha foi redefinida.";
                TempData[Globals.StatusMessageTypeKey] = Globals.StatusMessageTypeSuccess;

                return RedirectToPage("./Login");
            }

            IdentityResult result = await _userManager.ResetPasswordAsync(user, Input.Code, Input.Password);
            if (result.Succeeded)
            {
                TempData[Globals.StatusMessageKey] = "Sua senha foi redefinida.";
                TempData[Globals.StatusMessageTypeKey] = Globals.StatusMessageTypeSuccess;

                return RedirectToPage("./Login");
            }

            foreach (IdentityError error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return Page();
        }
    }
}
