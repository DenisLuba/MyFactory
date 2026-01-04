using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFactory.Domain.Entities.Security;

namespace MyFactory.Infrastructure.Persistence.Configurations.SecurityConfigurations;

public class TokenConfiguration : IEntityTypeConfiguration<TokenEntity>
{
    public void Configure(EntityTypeBuilder<TokenEntity> builder)
    {
        builder.ToTable("TOKENS");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.UserId).IsRequired();

        builder.Property(t => t.RefreshToken)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(t => t.ExpiresAt).IsRequired();
        builder.Property(t => t.RevokedAt);

        builder.HasIndex(t => t.RefreshToken).IsUnique();
    }
}

