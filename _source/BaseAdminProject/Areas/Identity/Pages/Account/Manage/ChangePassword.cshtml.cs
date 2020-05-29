// <copyright file="ChangePassword.cshtml.cs" company="Felipe Pergher">
// Copyright (c) Felipe Pergher. All Rights Reserved.
// </copyright>

using BaseAdminProject.Business.Core;
using BaseAdminProject.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace BaseAdminProject.Areas.Identity.Pages.Account.Manage
{
    public class ChangePasswordModel : PageModel
    {
        private readonly UserManager<BaseAdminUser> _userManager;
        private readonly SignInManager<BaseAdminUser> _signInManager;
        private readonly ILogger<ChangePasswordModel> _logger;

        public ChangePasswordModel(
            UserManager<BaseAdminUser> userManager,
            SignInManager<BaseAdminUser> signInManager,
            ILogger<ChangePasswordModel> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Display(Name = "Senha Atual")]
            [Required(ErrorMessage = Globals.RequiredMessage)]
            [PasswordPropertyText]
            [DataType(DataType.Password)]
            public string OldPassword { get; set; }

            [Display(Name = "Nova Senha")]
            [Required(ErrorMessage = Globals.RequiredMessage)]
            [PasswordPropertyText]
            [DataType(DataType.Password)]
            [RegularExpression(@"^(?=.*[a-z])(?=.*\d).{8,}$", ErrorMessage = "A senha deve conter letras, números e minimo de 8 caracteres")]
            public string NewPassword { get; set; }

            [Display(Name = "Confirmação da nova senha")]
            [Required(ErrorMessage = Globals.RequiredMessage)]
            [Compare(nameof(NewPassword), ErrorMessage = "As senhas não conferem.")]
            [PasswordPropertyText]
            [DataType(DataType.Password)]
            public string ConfirmPassword { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            BaseAdminUser user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            bool hasPassword = await _userManager.HasPasswordAsync(user);
            if (!hasPassword)
            {
                // Todo set password page
                return RedirectToPage("./SetPassword");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            BaseAdminUser user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            IdentityResult changePasswordResult = await _userManager.ChangePasswordAsync(user, Input.OldPassword, Input.NewPassword);
            if (!changePasswordResult.Succeeded)
            {
                foreach (IdentityError error in changePasswordResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                return Page();
            }

            await _signInManager.RefreshSignInAsync(user);
            _logger.LogInformation("User changed their password successfully.");

            TempData[Globals.StatusMessageKey] = "Sua senha foi atualizada com sucesso!";
            TempData[Globals.StatusMessageTypeKey] = Globals.StatusMessageTypeSuccess;

            return RedirectToPage();
        }
    }
}
