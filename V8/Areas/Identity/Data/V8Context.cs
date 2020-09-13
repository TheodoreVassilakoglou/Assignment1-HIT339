using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using V8.Models;

namespace V8.Data
{
    public class V8Context : IdentityDbContext<V8User>
    {
        public V8Context(DbContextOptions<V8Context> options)
            : base(options)
        {
        }

        public DbSet<Product> Product { get; set; }
        public DbSet<Sales> Sales { get; set; }
        public DbSet<Cart> Cart { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}

