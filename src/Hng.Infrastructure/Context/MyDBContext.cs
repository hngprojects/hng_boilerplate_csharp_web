using Hng.Domain.Entities;
using Microsoft.EntityFrameworkCore;

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
            .HasMany(u => u.Organisations)
            .WithMany(o => o.Users)
            .UsingEntity<OrganisationUser>(
                j => j
                    .HasOne(uo => uo.Organisation)
                    .WithMany(o => o.OrganisationUsers)
                    .HasForeignKey(uo => uo.OrganisationId)
                    .OnDelete(DeleteBehavior.Restrict),
                j => j
                    .HasOne(uo => uo.User)
                    .WithMany(u => u.OrganisationUsers)
                    .HasForeignKey(uo => uo.UserId)
                    .OnDelete(DeleteBehavior.Restrict),
                j =>
                {
                    j.HasKey(uo => uo.Id);
                    j.Property(uo => uo.Id).ValueGeneratedOnAdd();
                    j.HasIndex(uo => new { uo.UserId, uo.OrganisationId }).IsUnique();
                });
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<Organisation> Organisations { get; set; }
        public DbSet<Product> Products { get; set; }
    }
}
