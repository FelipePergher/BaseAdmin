// <copyright file="Roles.cs" company="Felipe Pergher">
// Copyright (c) Felipe Pergher. All Rights Reserved.
// </copyright>

namespace BaseAdminProject.Business.Core
{
    public static class Roles
    {
        public const string Admin = "Admin";

        public const string User = "User";

        public const string AdminUserAuthorize = "Admin, User";

        public const string AdminUserSocialAssistanceAuthorize = "Admin, User";

        public static string GetRoleName(string role)
        {
            string userRoleName = role switch
            {
                Admin => "Administrador",
                User => "Usuário",
                _ => string.Empty
            };

            return userRoleName;
        }
    }
}
