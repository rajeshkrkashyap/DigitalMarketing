using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Core.Shared.Entities;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace DAL
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<ApplicationService> Services { get; set; }
        public DbSet<Recharge> Recharges { get; set; }
 
        public DbSet<Project> Projects { get; set; }
        public DbSet<Crawled> Crawleds { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Define seed data using modelBuilder.Entity<YourEntity>().HasData()
            modelBuilder.Entity<Microsoft.AspNetCore.Identity.IdentityRole>().HasData(
               new Microsoft.AspNetCore.Identity.IdentityRole<string> { Id = Guid.NewGuid().ToString(), Name = "admin", NormalizedName = "ADMIN", ConcurrencyStamp = Guid.NewGuid().ToString() },
               new Microsoft.AspNetCore.Identity.IdentityRole<string> { Id = Guid.NewGuid().ToString(), Name = "student", NormalizedName = "STUDENT", ConcurrencyStamp = Guid.NewGuid().ToString() });

            modelBuilder.Entity<Product>().HasData(
                new Product { Id = Guid.NewGuid().ToString(), Title = "Connect To Ai", Description = "Educational Service", ImageUrl = "product1.jpg", Price = 5000, }
                );

            modelBuilder.Entity<ApplicationService>().HasData(
              new ApplicationService { Id = Guid.NewGuid().ToString(), Name = "Tokens", Description = "Educational Service", Cost = Convert.ToDecimal(0.004), Created = DateTime.UtcNow, Updated = DateTime.UtcNow, IsActive = true }
              );
        }
    }
}