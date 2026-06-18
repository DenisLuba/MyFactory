using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFactory.Domain.Entities.Materials;

namespace MyFactory.Infrastructure.Persistence.Configurations.MaterialConfigurations;

public class MaterialImageConfiguration : IEntityTypeConfiguration<MaterialImageEntity>
{
    public void Configure(EntityTypeBuilder<MaterialImageEntity> builder)
    {
        builder.ToTable("MATERIAL_IMAGES");

        builder.HasKey(mi => mi.Id);

        builder.Property(mi => mi.MaterialId).IsRequired();
        builder.Property(mi => mi.FileName).IsRequired().HasMaxLength(255);
        builder.Property(mi => mi.Path).IsRequired().HasMaxLength(500);
        builder.Property(mi => mi.ContentType).HasMaxLength(200);
        builder.Property(mi => mi.SortOrder).IsRequired();
    }
}
