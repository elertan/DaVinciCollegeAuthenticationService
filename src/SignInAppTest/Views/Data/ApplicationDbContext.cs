﻿using DaVinciCollegeAuthenticationService.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DaVinciCollegeAuthenticationService.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<Application> Applications { get; set; }
        public DbSet<Accesstoken> Accesstokens { get; set; }
        public DbSet<PasswordReset> PasswordResets { get; set; }

        public DbSet<ApplicationUserHasAuthLevel> ApplicationUserHasAuthLevels { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>()
                .HasMany(user => user.Applications);

            builder.Entity<Application>()
                .HasOne(app => app.User);

            builder.Entity<Application>()
                .HasMany(app => app.ApplicationUsersHasAuthLevels);

            builder.Entity<ApplicationUserHasAuthLevel>().HasOne(auhal => auhal.App);
        }
    }
}