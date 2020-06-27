// <copyright file="StatusCodeController.cs" company="Felipe Pergher">
// Copyright (c) Felipe Pergher. All Rights Reserved.
// </copyright>

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BaseAdminProject.Controllers
{
    [Authorize]
    public class StatusCodeController : Controller
    {
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Index(int code)
        {
            return code switch
            {
                404 => View("404"),
                500 => View("500"),
                _ => View("500")
            };
        }
    }
}
