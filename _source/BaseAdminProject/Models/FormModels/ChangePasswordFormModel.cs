// <copyright file="ChangePasswordFormModel.cs" company="Felipe Pergher">
// Copyright (c) Felipe Pergher. All Rights Reserved.
// </copyright>

using System.ComponentModel;
using BaseAdminProject.Business.Core;
using System.ComponentModel.DataAnnotations;

namespace BaseAdminProject.Models.FormModels
{
    public class ChangePasswordFormModel
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
}
