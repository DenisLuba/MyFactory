using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFactory.Domain.Entities.Inventory;

namespace MyFactory.Infrastructure.Persistence.Configurations.InventoryConfigurations;

public class InventoryMovementConfiguration : IEntityTypeConfiguration<InventoryMovementEntity>
{
    public void Configure(EntityTypeBuilder<InventoryMovementEntity> builder)
    {
        builder.ToTable("INVENTORY_MOVEMENTS");

        builder.HasKey(im => im.Id);

        builder.Property(im => im.MovementType).IsRequired();
        builder.Property(im => im.FromWarehouseId);
        builder.Property(im => im.ToWarehouseId);
        builder.Property(im => im.ToDepartmentId);
        builder.Property(im => im.ProductionOrderId);
        builder.Property(im => im.CreatedBy).IsRequired();

        builder.HasMany(im => im.InventoryMovementItems)
            .WithOne(i => i.Movement)
            .HasForeignKey(i => i.MovementId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(im => im.CreatedBy);
    }
}
