using Hng.Domain.Models;
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
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

             modelBuilder.Entity<User>()
                .HasMany(ur => ur.OrganisationUsers)
                .WithOne(u => u.User)
                .HasForeignKey(f => f.UserId)
                .IsRequired();

                 modelBuilder.Entity<User>()
                .HasMany(ur => ur.OrganisationUsers)
                .WithOne(u => u.User)
                .HasForeignKey(f => f.UserId)
                .IsRequired();
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<Organisation> Organisations { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<OrganisationUser> OrganisationUsers { get; set; }
    }
}
