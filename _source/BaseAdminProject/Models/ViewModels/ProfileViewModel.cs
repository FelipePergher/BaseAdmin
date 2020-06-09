// <copyright file="ProfileViewModel.cs" company="Felipe Pergher">
// Copyright (c) Felipe Pergher. All Rights Reserved.
// </copyright>

using BaseAdminProject.Models.FormModels;

namespace BaseAdminProject.Models.ViewModels
{
    public class ProfileViewModel
    {
        public PhoneFormModel PhoneForm { get; set; }

        public UsernameFormModel UsernameForm { get; set; }

        public EmailFormModel EmailForm { get; set; }
    }
}
