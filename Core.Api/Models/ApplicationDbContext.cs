using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using Microsoft.Extensions.Options;
using Core.Shared.Entities;
using System;
using System.Reflection.Emit;

namespace Core.Api.Models
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<ApplicationService> ApplicationServices { get; set; }
        public DbSet<Recharge> Recharges { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Crawled> Crawleds { get; set; }
        public DbSet<ContentAnalysis> ContentAnalysiss { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Define seed data using modelBuilder.Entity<YourEntity>().HasData()
            modelBuilder.Entity<Microsoft.AspNetCore.Identity.IdentityRole>().HasData(
               new Microsoft.AspNetCore.Identity.IdentityRole<string> { Id = "076B9864-AEA6-4745-8016-B84ABE8B800C", Name = "admin", NormalizedName = "ADMIN", ConcurrencyStamp = "6221e499-46e5-4f80-94d7-85d4df357e51" },
               new Microsoft.AspNetCore.Identity.IdentityRole<string> { Id = "975B70A6-6C6A-4C02-9570-42F9B15B4D74", Name = "student", NormalizedName = "STUDENT", ConcurrencyStamp = "c6525168-13b0-4735-9d4a-aedd547bb674" },
               new Microsoft.AspNetCore.Identity.IdentityRole<string> { Id = "CCAFEA64-3A85-49AA-BA4B-7D2E296226FA", Name = "marketing", NormalizedName = "MARKETING", ConcurrencyStamp = "48129C52-C42A-4C55-8448-5B1947403635" });


            modelBuilder.Entity<Product>().HasData(
                new Product { Id = "BEC293FA-B1E2-4B33-8FCC-375D83BF6246", Title = "Connect To Ai", Description = "Educational Service", ImageUrl = "product1.jpg", Price = 5000, }
                );

            //modelBuilder.Entity<ApplicationService>().HasData(
            //    new ApplicationService { Id = "34E91052-CC59-488F-8C5C-286C0D9AE55F", Name = "Tokens", Description = "Educational Service", Cost = Convert.ToDecimal(0.004), Created = DateTime.UtcNow, Updated = DateTime.UtcNow, IsActive = true }
            //    );
        }
    }
}
