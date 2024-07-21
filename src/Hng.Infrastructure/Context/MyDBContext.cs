using Hng.Domain.Entities;
using Hng.Infrastructure.Cofigurations;
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
            modelBuilder.ApplyConfiguration<NewsLetterSubscriber>(new NewsLetterSubscriberConfig());
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<NewsLetterSubscriber> NewsLetterSubscribers { get; set; }
    }
}
