// <copyright file="UserViewModel.cs" company="Felipe Pergher">
// Copyright (c) Felipe Pergher. All Rights Reserved.
// </copyright>

namespace BaseAdminProject.Models.ViewModels
{
    public class UserViewModel
    {
        public string UserId { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string ConfirmedAccount { get; set; }

        public string BlockedAccount { get; set; }

        public string Role { get; set; }
    }
}
