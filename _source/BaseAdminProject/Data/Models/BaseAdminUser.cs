// <copyright file="BaseAdminUser.cs" company="Felipe Pergher">
// Copyright (c) Felipe Pergher. All Rights Reserved.
// </copyright>

using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace BaseAdminProject.Data.Models
{
    public class BaseAdminUser : IdentityUser
    {
        public int UserInfoId { get; set; }

        [ForeignKey(nameof(UserInfoId))]
        public UserInfo UserInfo { get; set; }
    }
}
