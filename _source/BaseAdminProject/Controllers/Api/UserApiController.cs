// <copyright file="UserApiController.cs" company="Felipe Pergher">
// Copyright (c) Felipe Pergher. All Rights Reserved.
// </copyright>

using BaseAdminProject.Business.Core;
using BaseAdminProject.Data.Models;
using BaseAdminProject.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Threading.Tasks;

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
        public IActionResult GetAll()
        {
            string adminEmail = _configuration["Admin:Email"];

            var data = _userManager.Users
                .Include(x => x.UserInfo)
                .Where(x => x.Email.ToLower() != adminEmail.ToLower())
                .AsEnumerable()
                .Select(x =>
                {
                    bool isEmailConfirmed = _userManager.IsEmailConfirmedAsync(x).Result;
                    bool isLockout = x.LockoutEnd != null;
                    return new UserViewModel
                    {
                        UserId = x.Id,
                        Name = x.UserInfo.Name,
                        BirthdayDate = x.UserInfo.BirthdayDate.ToShortDateString(),
                        UserName = x.UserName,
                        Email = x.Email,
                        ConfirmedAccount = new DataTablesRender
                        {
                            Plain = isEmailConfirmed.ToString(),
                            Filter = isEmailConfirmed.ToString(),
                            Display = isEmailConfirmed
                                ? "<span class='fa fa-user-check text-success'></span>"
                                : string.Empty
                        },
                        BlockedAccount = new DataTablesRender
                        {
                            Plain = isLockout.ToString(),
                            Filter = isLockout.ToString(),
                            Display = isLockout
                                ? "<span class='fa fa-user-check text-success'></span>"
                                : string.Empty,
                        },
                        Active = new DataTablesRender
                        {
                            Plain = x.UserInfo.Active.ToString(),
                            Filter = x.UserInfo.Active.ToString(),
                            Display = x.UserInfo.Active
                                ? "<span class='fa fa-check text-success'></span>"
                                : "<span class='fa fa-times text-danger'></span>",
                        },
                        Role = Roles.GetRoleName(_userManager.GetRolesAsync(x).Result.FirstOrDefault()),
                        Actions = GetActions(x)
                    };
                }).ToList();

            return Ok(new { data });
        }

        [HttpPost("~/api/user/changeStatusUser")]
        public async Task<IActionResult> ChangeStatusUser(string id)
        {
            BaseAdminUser user = _userManager.Users.Include(x => x.UserInfo).FirstOrDefault(x => x.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            user.UserInfo.Active = !user.UserInfo.Active;
            IdentityResult result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return Ok(user.UserInfo.Active ? "O acesso do usuário foi ativado com sucesso." : "O acesso do usuário foi removido com sucesso.");
            }

            return StatusCode(500, "Alguma coisa deu errado! <br> Se o problema persistir contate o administrador!");
        }

        [HttpGet("~/api/user/isUsedUsername")]
        public IActionResult IsUsedUsername(string username, string userId)
        {
            BaseAdminUser user = _userManager.Users.FirstOrDefault(x => x.NormalizedUserName == username.ToUpper() && x.Id != userId);
            return Ok(user == null);
        }

        [HttpGet("~/api/user/isUsedEmail")]
        public IActionResult IsUsedEmail(string email, string userId)
        {
            BaseAdminUser user = _userManager.Users.FirstOrDefault(x => x.NormalizedEmail == email.ToUpper() && x.Id != userId);
            return Ok(user == null);
        }

        #region Private Methods

        private string GetActions(BaseAdminUser user)
        {
            string actions = user.UserInfo.Active
                ? $"<button class='btn btn-sm btn-primary disableUserButton' data-url='{Url.Action("ChangeStatusUser", "UserApi", new { id = user.Id })}'>Desativar</button>"
                : $"<button class='btn btn-sm btn-primary enableUserButton' data-url='{Url.Action("ChangeStatusUser", "UserApi", new { id = user.Id })}'>Ativar</button>";

            return actions;
        }

        #endregion
    }
}