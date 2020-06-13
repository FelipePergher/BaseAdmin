﻿// <copyright file="BaseAdminUser.cs" company="Felipe Pergher">
// Copyright (c) Felipe Pergher. All Rights Reserved.
// </copyright>

using Microsoft.AspNetCore.Identity;

namespace BaseAdminProject.Data.Models
{
    public class BaseAdminUser : IdentityUser
    {
        public BaseAdminUser()
        {
            UserInfo = new UserInfo();
        }

        public UserInfo UserInfo { get; set; }
    }
}
