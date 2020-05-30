// <copyright file="SetPassword.cshtml.cs" company="Felipe Pergher">
// Copyright (c) Felipe Pergher. All Rights Reserved.
// </copyright>

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BaseAdminProject.Areas.Identity.Pages.Account.Manage
{
    public class SetPasswordModel : PageModel
    {
        public IActionResult OnGet()
        {
            return RedirectToPage("./Index");
        }
    }
}
