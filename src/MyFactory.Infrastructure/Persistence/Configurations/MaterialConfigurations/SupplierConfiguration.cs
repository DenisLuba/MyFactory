using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFactory.Domain.Entities.Materials;

namespace MyFactory.Infrastructure.Persistence.Configurations.MaterialConfigurations;

public class SupplierConfiguration : IEntityTypeConfiguration<SupplierEntity>
{
    public void Configure(EntityTypeBuilder<SupplierEntity> builder)
    {
        builder.ToTable("SUPPLIERS");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(s => s.Description)
            .HasMaxLength(500);

        builder.Property(s => s.IsActive)
            .IsRequired();

        builder.HasIndex(s => s.Name).IsUnique();

        builder.HasMany(s => s.MaterialSuppliers)
            .WithOne(ms => ms.Supplier)
            .HasForeignKey(ms => ms.SupplierId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(s => s.MaterialPurchaseOrders)
            .WithOne(po => po.Supplier)
            .HasForeignKey(po => po.SupplierId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
