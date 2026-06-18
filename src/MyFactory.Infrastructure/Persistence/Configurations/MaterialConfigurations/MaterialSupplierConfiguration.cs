using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFactory.Domain.Entities.Materials;

namespace MyFactory.Infrastructure.Persistence.Configurations.MaterialConfigurations;

public class MaterialSupplierConfiguration : IEntityTypeConfiguration<MaterialSupplierEntity>
{
    public void Configure(EntityTypeBuilder<MaterialSupplierEntity> builder)
    {
        builder.ToTable("MATERIAL_SUPPLIERS");

        builder.HasKey(ms => ms.Id);

        builder.Property(ms => ms.MaterialId).IsRequired();
        builder.Property(ms => ms.SupplierId).IsRequired();
        builder.Property(ms => ms.MinOrderQty).HasColumnType("numeric(18,2)");
        builder.Property(ms => ms.IsActive).IsRequired();

        builder.HasIndex(ms => new { ms.MaterialId, ms.SupplierId }).IsUnique();
    }
}
