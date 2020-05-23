// <copyright file="AddUserFormModel.cs" company="Felipe Pergher">
// Copyright (c) Felipe Pergher. All Rights Reserved.
// </copyright>

using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using BaseAdminProject.Business.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BaseAdminProject.Models.FormModels
{
    public class AddUserFormModel
    {
        [Display(Name = "Nome de usuário")]
        [Required(ErrorMessage = Globals.RequiredMessage)]
        [Remote("IsUsedUsername", "UserApi", ErrorMessage = "Nome de usuário já utilizado!")]
        public string UserName { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = Globals.RequiredMessage)]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        [Remote("IsUsedEmail", "UserApi", ErrorMessage = "Email já utilizado!")]
        public string Email { get; set; }

        [Display(Name = "Número de telefone")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        public List<SelectListItem> ApplicationRoles => new List<SelectListItem>
        {
            new SelectListItem(Roles.GetRoleName(Roles.Admin), Roles.Admin),
            new SelectListItem(Roles.GetRoleName(Roles.User), Roles.User)
        };

        [Display(Name = "Regra de usuário")]
        [Required(ErrorMessage = Globals.RequiredMessage)]
        public string Role { get; set; }

        [Display(Name = "Senha")]
        [Required(ErrorMessage = Globals.RequiredMessage)]
        [PasswordPropertyText]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*\d).{8,}$", ErrorMessage = "A senha deve conter letras, números e minimo de 8 caracteres")]
        public string Password { get; set; }

        [Display(Name = "Confirmação de senha")]
        [Required(ErrorMessage = Globals.RequiredMessage)]
        [Compare(nameof(Password))]
        [PasswordPropertyText]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
