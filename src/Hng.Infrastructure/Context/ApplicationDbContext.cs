using Hng.Domain.Entities;
using Hng.Domain.EntitiesConfigurations;
using Hng.Domain.EntityConfigurations;
using Microsoft.EntityFrameworkCore;

namespace Hng.Infrastructure.Context
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration<NewsLetterSubscriber>(new NewsLetterSubscriberConfig());
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<NewsLetterSubscriber> NewsLetterSubscribers { get; set; }

    }
}
