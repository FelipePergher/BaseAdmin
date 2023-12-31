﻿// <copyright file="UserApiV1Controller.cs" company="Felipe Pergher">
// Copyright (c) Felipe Pergher. All Rights Reserved.
// </copyright>

using Microsoft.AspNetCore.Mvc;

namespace BaseAdminProject.Controllers.ApiMobile.V1
{
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/user/[action]")]
    [ControllerName("User Api")]
    public class UserApiV1Controller : ControllerBase
    {
        [HttpGet]
        public IActionResult Test()
        {
            return Ok("Testing version of apis - V1");
        }
    }
}