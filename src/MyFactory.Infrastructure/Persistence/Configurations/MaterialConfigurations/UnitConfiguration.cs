using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFactory.Domain.Entities.Materials;

namespace MyFactory.Infrastructure.Persistence.Configurations.MaterialConfigurations;

public class UnitConfiguration : IEntityTypeConfiguration<UnitEntity>
{
    public void Configure(EntityTypeBuilder<UnitEntity> builder)
    {
        builder.ToTable("UNITS");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Code)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(u => u.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.HasIndex(u => u.Code).IsUnique();

        builder.HasMany(u => u.Materials)
            .WithOne(m => m.Unit)
            .HasForeignKey(m => m.UnitId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
