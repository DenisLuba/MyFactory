using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFactory.Domain.Entities.Materials;

namespace MyFactory.Infrastructure.Persistence.Configurations.MaterialConfigurations;

public class MaterialTypeConfiguration : IEntityTypeConfiguration<MaterialTypeEntity>
{
    public void Configure(EntityTypeBuilder<MaterialTypeEntity> builder)
    {
        builder.ToTable("MATERIAL_TYPES");

        builder.HasKey(mt => mt.Id);

        builder.Property(mt => mt.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(mt => mt.Description)
            .HasMaxLength(500);

        builder.HasIndex(mt => mt.Name).IsUnique();

        builder.HasMany(mt => mt.Materials)
            .WithOne(m => m.MaterialType)
            .HasForeignKey(m => m.MaterialTypeId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
