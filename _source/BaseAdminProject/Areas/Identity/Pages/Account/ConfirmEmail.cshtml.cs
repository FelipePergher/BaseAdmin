// <copyright file="ConfirmEmail.cshtml.cs" company="Felipe Pergher">
// Copyright (c) Felipe Pergher. All Rights Reserved.
// </copyright>

using BaseAdminProject.Business.Core;
using BaseAdminProject.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using System.Threading.Tasks;

namespace BaseAdminProject.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ConfirmEmailModel : PageModel
    {
        private readonly UserManager<BaseAdminUser> _userManager;

        public ConfirmEmailModel(UserManager<BaseAdminUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> OnGetAsync(string userId, string code)
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

                    return RedirectToPage("./Login");
                }
            }

            TempData[Globals.StatusMessageKey] = "Erro ao confirmar seu email.";
            TempData[Globals.StatusMessageTypeKey] = Globals.StatusMessageTypeDanger;

            return RedirectToPage("./Login");
        }
    }
}
