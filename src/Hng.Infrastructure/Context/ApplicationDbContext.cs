using System.Text.Json;
using System.Text.Json.Serialization;
using Hng.Domain.Entities;
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
            modelBuilder.Entity<EmailTemplate>()
            .Property(e => e.PlaceHolders)
            .HasColumnType("jsonb") //Map to the native json type of PostgreSQL
            .HasConversion(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                v => JsonSerializer.Deserialize<Dictionary<string, string>>(v, (JsonSerializerOptions)null));
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<NewsLetterSubscriber> NewsLetterSubscribers { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<EmailTemplate> EmailTemplates { get; set; }
    }
}
