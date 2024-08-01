using Hng.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hng.Domain.EntitiesConfigurations
{
    public class RoleConfig : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name)
                .HasMaxLength(250)
                .IsRequired();
            builder.Property(x=>x.Description);
            builder.Property(x=> x.IsActive).IsRequired();
            builder.Property(x=>x.CreatedAt).IsRequired();
        }
    }
}
