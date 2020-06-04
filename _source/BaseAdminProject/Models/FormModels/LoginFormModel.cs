// <copyright file="LoginFormModel.cs" company="Felipe Pergher">
// Copyright (c) Felipe Pergher. All Rights Reserved.
// </copyright>

using BaseAdminProject.Business.Core;
using System.ComponentModel.DataAnnotations;

namespace BaseAdminProject.Models.FormModels
{
    public class LoginFormModel
    {
        [Required(ErrorMessage = Globals.RequiredMessage)]
        [Display(Name = "Email/Usuário")]
        public string EmailUsername { get; set; }

        [Required(ErrorMessage = Globals.RequiredMessage)]
        [Display(Name = "Senha")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Lembrar-me?")]
        public bool RememberMe { get; set; }
    }
}
