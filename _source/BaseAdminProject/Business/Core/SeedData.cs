// <copyright file="SeedData.cs" company="Felipe Pergher">
// Copyright (c) Felipe Pergher. All Rights Reserved.
// </copyright>

using BaseAdminProject.Data;
using BaseAdminProject.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BaseAdminProject.Business.Core
{
    public static class SeedData
    {
        private static readonly string[] Roles = { Core.Roles.Admin, Core.Roles.User };

        public static void ApplyMigrations(IServiceProvider serviceProvider)
        {
            using (IServiceScope serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                using (BaseAdminDbContext context = serviceScope.ServiceProvider.GetService<BaseAdminDbContext>())
                {
                    context.Database.Migrate();
                }
            }
        }

        public static async Task SeedRoles(IServiceScopeFactory scopeFactory)
        {
            using (IServiceScope serviceScope = scopeFactory.CreateScope())
            {
                BaseAdminDbContext dbContext = serviceScope.ServiceProvider.GetService<BaseAdminDbContext>();

                RoleManager<IdentityRole> roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                foreach (string role in Roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        await roleManager.CreateAsync(new IdentityRole
                        {
                            Name = role
                        });
                    }
                }
            }
        }

        public static async Task SeedAdminUser(IServiceScopeFactory scopeFactory, string email, string username, string password)
        {
            using (IServiceScope serviceScope = scopeFactory.CreateScope())
            {
                UserManager<BaseAdminUser> userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<BaseAdminUser>>();
                RoleManager<IdentityRole> roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                IList<BaseAdminUser> applicationUsers = await userManager.GetUsersInRoleAsync(Core.Roles.Admin);
                if (!applicationUsers.Any())
                {
                    var user = new BaseAdminUser
                    {
                        EmailConfirmed = true,
                        UserName = username,
                        Email = email,
                    };

                    user.UserInfo.Active = true;
                    user.UserInfo.Name = username;

                    IdentityResult result = await userManager.CreateAsync(user, password);

                    if (result.Succeeded)
                    {
                        IdentityRole applicationRole = await roleManager.FindByNameAsync(Core.Roles.Admin);
                        if (applicationRole != null)
                        {
                            await userManager.AddToRoleAsync(user, applicationRole.Name);
                        }
                    }
                }
            }
        }
    }
}
