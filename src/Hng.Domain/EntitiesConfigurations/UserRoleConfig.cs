using Hng.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hng.Domain.EntitiesConfigurations
{
    public class UserRoleConfig : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.HasKey(ur => ur.Id);
            builder.Property(ur => ur.UserId).IsRequired();
            builder.Property(ur => ur.RoleId).IsRequired();
            builder.HasIndex(ur => new
            {
                ur.OrganizationId,
                ur.UserId,
                ur.RoleId
            }).IsUnique();
            builder.HasOne(ur => ur.Role)
                .WithMany(r => r.UsersRoles)
                .HasForeignKey(ur => ur.RoleId);

            builder.HasOne(ur => ur.User)
                .WithMany(r => r.UsersRoles)
                .HasForeignKey(ur => ur.UserId);

            builder.HasOne(ur => ur.Orgainzation)
                .WithMany(r => r.UsersRoles)
                .HasForeignKey(ur => ur.OrganizationId);
        }
    }
}
