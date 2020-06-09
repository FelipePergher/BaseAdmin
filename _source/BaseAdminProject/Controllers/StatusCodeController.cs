// <copyright file="StatusCodeController.cs" company="Felipe Pergher">
// Copyright (c) Felipe Pergher. All Rights Reserved.
// </copyright>

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BaseAdminProject.Controllers
{
    [Authorize]
    public class StatusCodeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public StatusCodeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Index(int code)
        {
            switch (code)
            {
                case 404:
                    return View("404");
                case 500:
                    return View("500");
                default:
                    return View("500");
            }
        }
    }
}
