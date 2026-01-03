using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFactory.Domain.Entities.Inventory;

namespace MyFactory.Infrastructure.Persistence.Configurations.InventoryConfigurations;

public class WarehouseMaterialConfiguration : IEntityTypeConfiguration<WarehouseMaterialEntity>
{
    public void Configure(EntityTypeBuilder<WarehouseMaterialEntity> builder)
    {
        builder.ToTable("WAREHOUSE_MATERIALS");

        builder.HasKey(wm => wm.Id);

        builder.Property(wm => wm.WarehouseId).IsRequired();
        builder.Property(wm => wm.MaterialId).IsRequired();
        builder.Property(wm => wm.Qty).IsRequired().HasColumnType("numeric(18,2)");

        builder.HasIndex(wm => new { wm.WarehouseId, wm.MaterialId }).IsUnique();
    }
}
