using Hng.Domain.Entities;
using Hng.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Hng.Domain.EntitiesConfigurations
{
    public class ApiStatusConfiguration : IEntityTypeConfiguration<ApiStatus>
    {
        public void Configure(EntityTypeBuilder<ApiStatus> builder)
        {
            builder
                .HasKey(i => i.Id);

            builder
                .Property(p => p.Status)
                .HasConversion(new EnumToStringConverter<ApiStatusType>());

            builder
                .Property(a => a.ApiGroup)
                .HasMaxLength(100);
        }
    }
}