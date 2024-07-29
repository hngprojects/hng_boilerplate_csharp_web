using Hng.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hng.Domain.EntityConfigurations
{
    public class NewsLetterSubscriberConfig : IEntityTypeConfiguration<NewsLetterSubscriber>
    {
        public void Configure(EntityTypeBuilder<NewsLetterSubscriber> builder)
        {
            builder.HasKey(nl => nl.Id);
            builder.HasIndex(nl => nl.Email)
                .IsUnique();
            builder.Property(nl => nl.Email)
                .HasMaxLength(150)
                .IsRequired();
        }
    }
}