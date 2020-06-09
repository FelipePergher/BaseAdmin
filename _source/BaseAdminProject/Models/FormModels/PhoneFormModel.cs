// <copyright file="PhoneFormModel.cs" company="Felipe Pergher">
// Copyright (c) Felipe Pergher. All Rights Reserved.
// </copyright>

using System.ComponentModel.DataAnnotations;

namespace BaseAdminProject.Models.FormModels
{
    public class PhoneFormModel
    {
        [Phone]
        [Display(Name = "Número de telefone")]
        [StringLength(16, MinimumLength = 16, ErrorMessage = "Insira um número de telefone válido.")]
        public string PhoneNumber { get; set; }
    }
}