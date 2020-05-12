// <copyright file="BaseAdminDbContext.cs" company="Felipe Pergher">
// Copyright (c) Felipe Pergher. All Rights Reserved.
// </copyright>

using BaseAdminProject.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BaseAdminProject.Data
{
    public class BaseAdminDbContext : IdentityDbContext<BaseAdminUser>
    {
        public BaseAdminDbContext(DbContextOptions<BaseAdminDbContext> options)
            : base(options)
        {
        }
    }
}
