// <copyright file="UserInfo.cs" company="Felipe Pergher">
// Copyright (c) Felipe Pergher. All Rights Reserved.
// </copyright>

using System;
using System.ComponentModel.DataAnnotations;

namespace BaseAdminProject.Data.Models
{
    public class UserInfo
    {
        [Key]
        public int UserInfoId { get; set; }

        public bool Active { get; set; }

        public DateTime DisabledDate { get; set; }

        [StringLength(200)]
        public string Name { get; set; }

        public DateTime BirthdayDate { get; set; }

        public BaseAdminUser BaseAdminUser { get; set; }
    }
}
