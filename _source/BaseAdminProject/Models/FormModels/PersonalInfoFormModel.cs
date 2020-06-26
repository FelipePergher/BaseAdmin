// <copyright file="PersonalInfoFormModel.cs" company="Felipe Pergher">
// Copyright (c) Felipe Pergher. All Rights Reserved.
// </copyright>

using BaseAdminProject.Business.Core;
using System.ComponentModel.DataAnnotations;

namespace BaseAdminProject.Models.FormModels
{
    public class PersonalInfoFormModel
    {
        [Display(Name = "Nome Completo")]
        [Required(ErrorMessage = Globals.RequiredMessage)]
        [DataType(DataType.Text)]
        [StringLength(20, ErrorMessage = "Limite de caracteres excede o máximo ({1}).")]
        public string Name { get; set; }

        [Display(Name = "Data de nascimento")]
        [Required(ErrorMessage = Globals.RequiredMessage)]
        [DataType(DataType.Text)]
        public string BirthdayDate { get; set; }
    }
}