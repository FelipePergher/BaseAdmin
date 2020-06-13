// <copyright file="UsernameFormModel.cs" company="Felipe Pergher">
// Copyright (c) Felipe Pergher. All Rights Reserved.
// </copyright>

using BaseAdminProject.Business.Core;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace BaseAdminProject.Models.FormModels
{
    public class UsernameFormModel
    {
        [HiddenInput]
        public string UserId { get; set; }

        [Display(Name = "Usuário")]
        [Required(ErrorMessage = Globals.RequiredMessage)]
        [RegularExpression(@"^[a-zA-Z0-9][.a-zA-Z0-9]{3,18}[a-zA-Z0-9]$", ErrorMessage = "O nome de usuário só pode conter letras e números e deve conter entre 5 e 18 caracteres.")]
        [Remote("IsUsedUsername", "UserApi", AdditionalFields = nameof(UserId), ErrorMessage = "Nome de usuário já utilizado!")]
        public string UserName { get; set; }
    }
}