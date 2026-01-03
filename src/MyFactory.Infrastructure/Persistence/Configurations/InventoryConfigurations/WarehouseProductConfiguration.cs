using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFactory.Domain.Entities.Inventory;

namespace MyFactory.Infrastructure.Persistence.Configurations.InventoryConfigurations;

public class WarehouseProductConfiguration : IEntityTypeConfiguration<WarehouseProductEntity>
{
    public void Configure(EntityTypeBuilder<WarehouseProductEntity> builder)
    {
        builder.ToTable("WAREHOUSE_PRODUCTS");

        builder.HasKey(wp => wp.Id);

        builder.Property(wp => wp.WarehouseId).IsRequired();
        builder.Property(wp => wp.ProductId).IsRequired();
        builder.Property(wp => wp.Qty).IsRequired();

        builder.HasIndex(wp => new { wp.WarehouseId, wp.ProductId }).IsUnique();
    }
}
