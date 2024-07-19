using Hng.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hng.Infrastructure.Context
{
    public class MyDBContext : DbContext
    {
        public MyDBContext(DbContextOptions<MyDBContext> options) : base(options)
        {
        }

        public DbSet<WaitlistUser> WaitlistUsers { get; set; }
        public DbSet<RateLimit> RateLimitUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RateLimit>().ToTable("RateLimits");
            modelBuilder.Entity<WaitlistUser>().ToTable("WaitlistUsers");
            base.OnModelCreating(modelBuilder);
        }
    }
}
