using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFactory.Domain.Entities.Inventory;

namespace MyFactory.Infrastructure.Persistence.Configurations.InventoryConfigurations;

public class InventoryMovementItemConfiguration : IEntityTypeConfiguration<InventoryMovementItemEntity>
{
    public void Configure(EntityTypeBuilder<InventoryMovementItemEntity> builder)
    {
        builder.ToTable("INVENTORY_MOVEMENT_ITEMS");

        builder.HasKey(i => i.Id);

        builder.Property(i => i.MovementId).IsRequired();
        builder.Property(i => i.MaterialId).IsRequired();
        builder.Property(i => i.Qty).IsRequired().HasColumnType("numeric(18,2)");
        builder.Property(i => i.UnitCost).IsRequired().HasColumnType("numeric(18,2)");
    }
}
