using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFactory.Domain.Entities.Materials;

namespace MyFactory.Infrastructure.Persistence.Configurations.MaterialConfigurations;

public class MaterialPurchaseOrderItemConfiguration : IEntityTypeConfiguration<MaterialPurchaseOrderItemEntity>
{
    public void Configure(EntityTypeBuilder<MaterialPurchaseOrderItemEntity> builder)
    {
        builder.ToTable("MATERIAL_PURCHASE_ITEMS");

        builder.HasKey(mpi => mpi.Id);

        builder.Property(mpi => mpi.PurchaseOrderId).IsRequired();
        builder.Property(mpi => mpi.MaterialId).IsRequired();
        builder.Property(mpi => mpi.Qty).IsRequired().HasColumnType("numeric(18,2)");
        builder.Property(mpi => mpi.UnitPrice).IsRequired().HasColumnType("numeric(18,2)");
        builder.Property(mpi => mpi.MaterialName).IsRequired().HasMaxLength(200);
        builder.Property(mpi => mpi.UnitCode).IsRequired().HasMaxLength(50);
    }
}
