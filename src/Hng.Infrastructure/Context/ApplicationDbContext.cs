using Hng.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Hng.Infrastructure.Context
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)
                      .UseIdentityColumn();
            });

            modelBuilder.Entity<Product>()
           .HasMany(p => p.Categories)
           .WithMany(c => c.Products)
           .UsingEntity(j => j.ToTable("ProductCategories"));

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Product> Products { get; set; }
    }
}
