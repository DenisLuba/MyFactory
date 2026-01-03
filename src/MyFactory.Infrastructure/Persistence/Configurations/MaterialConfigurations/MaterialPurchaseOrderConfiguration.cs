using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFactory.Domain.Entities.Materials;

namespace MyFactory.Infrastructure.Persistence.Configurations.MaterialConfigurations;

public class MaterialPurchaseOrderConfiguration : IEntityTypeConfiguration<MaterialPurchaseOrderEntity>
{
    public void Configure(EntityTypeBuilder<MaterialPurchaseOrderEntity> builder)
    {
        builder.ToTable("MATERIAL_PURCHASE_ORDERS");

        builder.HasKey(po => po.Id);

        builder.Property(po => po.SupplierId).IsRequired();
        builder.Property(po => po.OrderDate).IsRequired();
        builder.Property(po => po.Status).IsRequired();

        builder.HasIndex(po => new { po.SupplierId, po.OrderDate });

        builder.HasMany(po => po.MaterialPurchaseItems)
            .WithOne(mpi => mpi.MaterialPurchaseOrder)
            .HasForeignKey(mpi => mpi.PurchaseOrderId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
