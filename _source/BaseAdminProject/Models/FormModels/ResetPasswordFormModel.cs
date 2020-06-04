// <copyright file="ResetPasswordFormModel.cs" company="Felipe Pergher">
// Copyright (c) Felipe Pergher. All Rights Reserved.
// </copyright>

using BaseAdminProject.Business.Core;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BaseAdminProject.Models.FormModels
{
    public class ResetPasswordFormModel
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
}
