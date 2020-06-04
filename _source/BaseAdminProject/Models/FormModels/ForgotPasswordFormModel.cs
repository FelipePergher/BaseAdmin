﻿// <copyright file="ForgotPasswordFormModel.cs" company="Felipe Pergher">
// Copyright (c) Felipe Pergher. All Rights Reserved.
// </copyright>

using BaseAdminProject.Business.Core;
using System.ComponentModel.DataAnnotations;

namespace BaseAdminProject.Models.FormModels
{
    public class ForgotPasswordFormModel
    {
        [Required(ErrorMessage = Globals.RequiredMessage)]
        [EmailAddress(ErrorMessage = Globals.EmailRequiredMessage)]
        public string Email { get; set; }
    }
}
