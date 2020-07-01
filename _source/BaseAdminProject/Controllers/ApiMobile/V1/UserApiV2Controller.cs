// <copyright file="UserApiV2Controller.cs" company="Felipe Pergher">
// Copyright (c) Felipe Pergher. All Rights Reserved.
// </copyright>

using Microsoft.AspNetCore.Mvc;

namespace BaseAdminProject.Controllers.ApiMobile.V1
{
    [ApiController]
    [ApiVersion("2")]
    [Route("api/v{version:apiVersion}/user/[action]")]
    [ControllerName("User Api")]
    public class UserApiV2Controller : ControllerBase
    {
        [HttpGet]
        public IActionResult Test()
        {
            return Ok("Testing version of apis- V2");
        }
    }
}