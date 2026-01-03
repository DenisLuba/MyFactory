using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFactory.Domain.Entities.Materials;

namespace MyFactory.Infrastructure.Persistence.Configurations.MaterialConfigurations;

public class MaterialConfiguration : IEntityTypeConfiguration<MaterialEntity>
{
    public void Configure(EntityTypeBuilder<MaterialEntity> builder)
    {
        builder.ToTable("MATERIALS");

        builder.HasKey(m => m.Id);

        builder.Property(m => m.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(m => m.MaterialTypeId).IsRequired();
        builder.Property(m => m.UnitId).IsRequired();
        builder.Property(m => m.Color).HasMaxLength(100);
        builder.Property(m => m.IsActive).IsRequired();

        builder.HasIndex(m => m.Name).IsUnique();

        builder.HasMany(m => m.ProductMaterials)
            .WithOne(pm => pm.Material)
            .HasForeignKey(pm => pm.MaterialId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(m => m.WarehouseMaterials)
            .WithOne(wm => wm.Material)
            .HasForeignKey(wm => wm.MaterialId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(m => m.MaterialSuppliers)
            .WithOne(ms => ms.Material)
            .HasForeignKey(ms => ms.MaterialId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(m => m.InventoryMovementItems)
            .WithOne(mi => mi.Material)
            .HasForeignKey(mi => mi.MaterialId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(m => m.MaterialPurchaseOrderItems)
            .WithOne(mpoi => mpoi.Material)
            .HasForeignKey(mpoi => mpoi.MaterialId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
