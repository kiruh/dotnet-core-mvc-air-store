using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AirStore.Models;

namespace AirStore.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
            builder.Entity<Comment>()
                .Property(b => b.CreatedAt)
                .HasDefaultValueSql("getdate()");
        }

        public DbSet<AirStore.Models.Air> Air { get; set; }
        public DbSet<AirStore.Models.Order> Order { get; set; }
        public DbSet<AirStore.Models.Comment> Comment { get; set; }
    }
}
