// <copyright file="IdentityHostingStartup.cs" company="Felipe Pergher">
// Copyright (c) Felipe Pergher. All Rights Reserved.
// </copyright>

using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(BaseAdminProject.Areas.Identity.IdentityHostingStartup))]

namespace BaseAdminProject.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
            });
        }
    }
}