using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFactory.Domain.Entities.Inventory;

namespace MyFactory.Infrastructure.Persistence.Configurations.InventoryConfigurations;

public class WarehouseConfiguration : IEntityTypeConfiguration<WarehouseEntity>
{
    public void Configure(EntityTypeBuilder<WarehouseEntity> builder)
    {
        builder.ToTable("WAREHOUSES");

        builder.HasKey(w => w.Id);

        builder.Property(w => w.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(w => w.Type)
            .IsRequired();

        builder.Property(w => w.IsActive)
            .IsRequired();

        builder.HasIndex(w => w.Name).IsUnique();

        builder.HasMany(w => w.WarehouseMaterials)
            .WithOne(wm => wm.Warehouse)
            .HasForeignKey(wm => wm.WarehouseId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(w => w.InventoryMovementsFrom)
            .WithOne(im => im.FromWarehouse)
            .HasForeignKey(im => im.FromWarehouseId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(w => w.InventoryMovementsTo)
            .WithOne(im => im.ToWarehouse)
            .HasForeignKey(im => im.ToWarehouseId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(w => w.FinishedGoods)
            .WithOne(fg => fg.Warehouse)
            .HasForeignKey(fg => fg.WarehouseId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(w => w.FinishedGoodsMovementsFrom)
            .WithOne(fgm => fgm.FromWarehouse)
            .HasForeignKey(fgm => fgm.FromWarehouseId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(w => w.FinishedGoodsMovementsTo)
            .WithOne(fgm => fgm.ToWarehouse)
            .HasForeignKey(fgm => fgm.ToWarehouseId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(w => w.FinishedGoodsStocks)
            .WithOne(fgs => fgs.Warehouse)
            .HasForeignKey(fgs => fgs.WarehouseId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(w => w.WarehouseProducts)
            .WithOne(wp => wp.Warehouse)
            .HasForeignKey(wp => wp.WarehouseId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(w => w.ShipmentItems)
            .WithOne(si => si.Warehouse)
            .HasForeignKey(si => si.WarehouseId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(w => w.ShipmentReturnItems)
            .WithOne(sri => sri.Warehouse)
            .HasForeignKey(sri => sri.WarehouseId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
