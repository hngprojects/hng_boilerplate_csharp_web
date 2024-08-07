using System.Text.Json;
using Hng.Domain.Entities;
using Hng.Domain.EntitiesConfigurations;
using Microsoft.EntityFrameworkCore;

namespace Hng.Infrastructure.Context
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration<NewsLetterSubscriber>(new NewsLetterSubscriberConfig());
            modelBuilder.ApplyConfiguration<Role>(new RoleConfig());
            modelBuilder.ApplyConfiguration<RolePermission>(new RolePermissionConfig());
            modelBuilder.ApplyConfiguration<UserRole>(new UserRoleConfig());
            modelBuilder.ApplyConfiguration<Transaction>(new PaymentConfiguration());
            modelBuilder.ApplyConfiguration<Subscription>(new SubscriptionConfiguration());
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
        public DbSet<NotificationSettings> NotificationSettings { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Faq> FAQ { get; set; }


    }
}
