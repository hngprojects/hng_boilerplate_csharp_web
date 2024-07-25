using Hng.Domain.Entities;
using Hng.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Hng.Domain.EntitiesConfigurations
{
    public class PaymentConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.Property(p => p.Type).HasConversion(new EnumToStringConverter<TransactionType>());
            builder.Property(p => p.Status).HasConversion(new EnumToStringConverter<TransactionStatus>());
            builder.Property(p => p.Partners).HasConversion(new EnumToStringConverter<TransactionIntegrationPartners>());

            builder.HasIndex(r => r.Reference);
        }
    }
}
