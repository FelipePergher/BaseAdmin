// <copyright file="Index.cshtml.cs" company="Felipe Pergher">
// Copyright (c) Felipe Pergher. All Rights Reserved.
// </copyright>

using BaseAdminProject.Business.Core;
using BaseAdminProject.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace BaseAdminProject.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<BaseAdminUser> _userManager;
        private readonly SignInManager<BaseAdminUser> _signInManager;

        public IndexModel(
            UserManager<BaseAdminUser> userManager,
            SignInManager<BaseAdminUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Phone]
            [Display(Name = "Número de telefone")]
            [StringLength(16, MinimumLength = 16, ErrorMessage = "Insira um número de telefone válido.")]
            public string PhoneNumber { get; set; }

            [Display(Name = "Usuário")]
            [Required(ErrorMessage = Globals.RequiredMessage)]
            [RegularExpression(@"^[a-zA-Z0-9][.a-zA-Z0-9]{3,18}[a-zA-Z0-9]$", ErrorMessage = "O nome de usuário só pode conter letras e números e deve conter entre 5 e 18 caracteres.")]
            public string UserName { get; set; }
        }

        private async Task LoadAsync(BaseAdminUser user)
        {
            string userName = await _userManager.GetUserNameAsync(user);
            string phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            Input = new InputModel
            {
                PhoneNumber = phoneNumber,
                UserName = userName
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            BaseAdminUser user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return BadRequest();
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            BaseAdminUser user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            // Todo make form to save another information of user
            TempData[Globals.StatusMessageKey] = "Seu perfil foi atualizado!";
            TempData[Globals.StatusMessageTypeKey] = Globals.StatusMessageTypeSuccess;

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostChangePhoneAsync()
        {
            BaseAdminUser user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            string phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                IdentityResult setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    TempData[Globals.StatusMessageKey] = "Alguma coisa deu errado salvando o telefone!";
                    TempData[Globals.StatusMessageTypeKey] = Globals.StatusMessageTypeDanger;
                    return RedirectToPage();
                }

                await _signInManager.RefreshSignInAsync(user);

                TempData[Globals.StatusMessageKey] = "Seu número de telefone foi atualizado!";
                TempData[Globals.StatusMessageTypeKey] = Globals.StatusMessageTypeSuccess;
                return RedirectToPage();
            }

            TempData[Globals.StatusMessageKey] = "Seu número de telefone não foi alterado.";
            TempData[Globals.StatusMessageTypeKey] = Globals.StatusMessageTypeInfo;

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostChangeUsernameAsync()
        {
            BaseAdminUser user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            string userName = await _userManager.GetUserNameAsync(user);

            if (Input.UserName != userName)
            {
                IdentityResult setPhoneResult = await _userManager.SetUserNameAsync(user, Input.UserName);
                if (!setPhoneResult.Succeeded)
                {
                    TempData[Globals.StatusMessageKey] = "Alguma coisa deu errado salvando o telefone!";
                    TempData[Globals.StatusMessageTypeKey] = Globals.StatusMessageTypeDanger;
                    return RedirectToPage();
                }

                await _signInManager.RefreshSignInAsync(user);

                TempData[Globals.StatusMessageKey] = "Seu usuário foi atualizado!";
                TempData[Globals.StatusMessageTypeKey] = Globals.StatusMessageTypeSuccess;
                return RedirectToPage();
            }

            TempData[Globals.StatusMessageKey] = "Seu usuário não foi alterado.";
            TempData[Globals.StatusMessageTypeKey] = Globals.StatusMessageTypeInfo;
            return RedirectToPage();
        }
    }
}
