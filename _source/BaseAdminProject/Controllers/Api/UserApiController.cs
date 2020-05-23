// <copyright file="UserApiController.cs" company="Felipe Pergher">
// Copyright (c) Felipe Pergher. All Rights Reserved.
// </copyright>

using System.Collections.Generic;
using System.Linq;
using BaseAdminProject.Business.Core;
using BaseAdminProject.Data.Models;
using BaseAdminProject.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace BaseAdminProject.Controllers.Api
{
    [Authorize(Roles = Roles.Admin)]
    [ApiController]
    public class UserApiController : ControllerBase
    {
        private readonly UserManager<BaseAdminUser> _userManager;
        private readonly IConfiguration _configuration;

        public UserApiController(UserManager<BaseAdminUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        [HttpGet("~/api/user/getAll")]
        public IActionResult UserGetAll()
        {
            string adminEmail = _configuration["Admin:Email"];

            IEnumerable<UserViewModel> data = _userManager.Users.Where(x => x.Email.ToLower() != adminEmail.ToLower()).ToList().Select(x => new UserViewModel
            {
                UserId = x.Id,
                Name = x.UserName,
                Email = x.Email,
                ConfirmedAccount = _userManager.IsEmailConfirmedAsync(x).Result ? "<span class='fa fa-user-check text-success'></span>" : string.Empty,
                BlockedAccount = x.LockoutEnd != null ? "<span class='fa fa-user-check text-success'></span>" : string.Empty,
                Role = Roles.GetRoleName(_userManager.GetRolesAsync(x).Result.FirstOrDefault()),
            }).ToList();

            return Ok(new { data });
        }

        [HttpGet("~/api/user/isUsedUsername")]
        public IActionResult IsUsedUsername(string username, string userId)
        {
            var user = _userManager.Users.FirstOrDefault(x => x.NormalizedUserName == username.ToUpper() && x.Id != userId);
            return Ok(user == null);
        }

        [HttpGet("~/api/user/isUsedEmail")]
        public IActionResult IsUsedEmail(string email, string userId)
        {
            var user = _userManager.Users.FirstOrDefault(x => x.NormalizedEmail == email.ToUpper() && x.Id != userId);
            return Ok(user == null);
        }
    }
}