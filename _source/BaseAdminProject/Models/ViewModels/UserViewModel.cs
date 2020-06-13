// <copyright file="UserViewModel.cs" company="Felipe Pergher">
// Copyright (c) Felipe Pergher. All Rights Reserved.
// </copyright>

namespace BaseAdminProject.Models.ViewModels
{
    public class UserViewModel
    {
        public string UserId { get; set; }

        public string Name { get; set; }

        public string UserName { get; set; }

        public string BirthdayDate { get; set; }

        public string Email { get; set; }

        public DataTablesRender ConfirmedAccount { get; set; }

        public DataTablesRender BlockedAccount { get; set; }

        public DataTablesRender Active { get; set; }

        public string Role { get; set; }

        public string Actions { get; set; }
    }
}
