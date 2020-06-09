// <copyright file="EmailFormModel.cs" company="Felipe Pergher">
// Copyright (c) Felipe Pergher. All Rights Reserved.
// </copyright>

using System.ComponentModel.DataAnnotations;
using BaseAdminProject.Business.Core;
using Microsoft.AspNetCore.Mvc;

namespace BaseAdminProject.Models.FormModels
{
    public class EmailFormModel
    {
        [HiddenInput]
        public string UserId { get; set; }

        [Required(ErrorMessage = Globals.RequiredMessage)]
        [EmailAddress(ErrorMessage = Globals.EmailRequiredMessage)]
        [Display(Name = "Email")]
        [Remote("IsUsedEmail", "UserApi", AdditionalFields = nameof(UserId), ErrorMessage = "Email já utilizado!")]
        public string Email { get; set; }

        [HiddenInput]
        public bool IsConfirmed { get; set; }
    }
}