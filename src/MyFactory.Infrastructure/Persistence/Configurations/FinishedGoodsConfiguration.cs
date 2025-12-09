using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFactory.Domain.Entities.FinishedGoods;
using MyFactory.Infrastructure.Persistence.Constants;

namespace MyFactory.Infrastructure.Persistence.Configurations;

public class FinishedGoodsInventoryConfiguration : IEntityTypeConfiguration<FinishedGoodsInventory>
{
    public void Configure(EntityTypeBuilder<FinishedGoodsInventory> builder)
    {
        builder.ToTable("FinishedGoodsInventory");

        builder.HasKey(inventory => inventory.Id);

        builder.Property(inventory => inventory.Quantity)
            .HasColumnType(ColumnTypes.Quantity)
            .IsRequired();

        builder.Property(inventory => inventory.UnitCost)
            .HasColumnType(ColumnTypes.MonetaryHighPrecision)
            .IsRequired();

        builder.Property(inventory => inventory.UpdatedAt)
            .IsRequired();

        builder.HasOne(inventory => inventory.Specification)
            .WithMany()
            .HasForeignKey(inventory => inventory.SpecificationId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(inventory => inventory.Warehouse)
            .WithMany()
            .HasForeignKey(inventory => inventory.WarehouseId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(inventory => new { inventory.SpecificationId, inventory.WarehouseId })
            .IsUnique();
    }
}

public class FinishedGoodsMovementConfiguration : IEntityTypeConfiguration<FinishedGoodsMovement>
{
    public void Configure(EntityTypeBuilder<FinishedGoodsMovement> builder)
    {
        builder.ToTable("FinishedGoodsMovements");

        builder.HasKey(movement => movement.Id);

        builder.Property(movement => movement.Quantity)
            .HasColumnType(ColumnTypes.Quantity)
            .IsRequired();

        builder.Property(movement => movement.MovedAt)
            .IsRequired();

        builder.HasOne(movement => movement.Specification)
            .WithMany()
            .HasForeignKey(movement => movement.SpecificationId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(movement => movement.FromWarehouse)
            .WithMany()
            .HasForeignKey(movement => movement.FromWarehouseId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(movement => movement.ToWarehouse)
            .WithMany()
            .HasForeignKey(movement => movement.ToWarehouseId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(movement => movement.FinishedGoodsInventory)
            .WithMany()
            .HasForeignKey(movement => movement.FinishedGoodsInventoryId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
