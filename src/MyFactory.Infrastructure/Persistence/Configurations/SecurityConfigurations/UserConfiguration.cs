using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFactory.Domain.Entities.Security;

namespace MyFactory.Infrastructure.Persistence.Configurations.SecurityConfigurations;

public class UserConfiguration : IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder.ToTable("USERS");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Username)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(u => u.PasswordHash)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(u => u.RoleId).IsRequired();

        builder.HasIndex(u => u.Username).IsUnique();

        builder.HasMany(u => u.ContactLinks)
            .WithOne()
            .HasForeignKey(cl => cl.OwnerId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(u => u.Tokens)
            .WithOne(t => t.User)
            .HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
