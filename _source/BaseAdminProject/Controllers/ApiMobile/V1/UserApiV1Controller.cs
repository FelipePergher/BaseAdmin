// <copyright file="UserApiV1Controller.cs" company="Felipe Pergher">
// Copyright (c) Felipe Pergher. All Rights Reserved.
// </copyright>

using BaseAdminProject.Business.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BaseAdminProject.Controllers.ApiMobile.V1
{
    [Authorize(Roles = Roles.Admin)]
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/user/[action]")]
    public class UserApiV1Controller : ControllerBase
    {
        [HttpGet]
        public IActionResult Test()
        {
            return Ok("Testing version of apis");
        }
    }
}