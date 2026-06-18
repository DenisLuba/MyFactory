using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFactory.Domain.Entities.Products;

namespace MyFactory.Infrastructure.Persistence.Configurations.ProductsConfigurations;

public class ProductMaterialConfiguration : IEntityTypeConfiguration<ProductMaterialEntity>
{
    public void Configure(EntityTypeBuilder<ProductMaterialEntity> builder)
    {
        builder.ToTable("PRODUCT_MATERIALS");

        builder.HasKey(pm => pm.Id);

        builder.Property(pm => pm.ProductId).IsRequired();
        builder.Property(pm => pm.MaterialId).IsRequired();
        builder.Property(pm => pm.QtyPerUnit).IsRequired().HasColumnType("numeric(18,2)");

        builder.HasIndex(pm => new { pm.ProductId, pm.MaterialId }).IsUnique();
    }
}
