// <copyright file="UserApiController.cs" company="Felipe Pergher">
// Copyright (c) Felipe Pergher. All Rights Reserved.
// </copyright>

using System.Collections.Generic;
using System.Linq;
using BaseAdminProject.Business.Core;
using BaseAdminProject.Data.Models;
using BaseAdminProject.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace BaseAdminProject.Controllers.Api
{
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

            IEnumerable<UserViewModel> data = _userManager.Users.Where(x => x.Email.ToLower() == adminEmail.ToLower()).ToList().Select(x => new UserViewModel
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
    }
}