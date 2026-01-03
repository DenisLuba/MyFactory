using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFactory.Domain.Entities.Production;

namespace MyFactory.Infrastructure.Persistence.Configurations.ProductionConfigurations;

public class ProductionOrderConfiguration : IEntityTypeConfiguration<ProductionOrderEntity>
{
    public void Configure(EntityTypeBuilder<ProductionOrderEntity> builder)
    {
        builder.ToTable("PRODUCTION_ORDERS");

        builder.HasKey(po => po.Id);

        builder.Property(po => po.ProductionOrderNumber)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(po => po.SalesOrderItemId).IsRequired();
        builder.Property(po => po.DepartmentId).IsRequired();
        builder.Property(po => po.QtyPlanned).IsRequired();
        builder.Property(po => po.QtyFinished).IsRequired();
        builder.Property(po => po.QtyCut).IsRequired();
        builder.Property(po => po.QtySewn).IsRequired();
        builder.Property(po => po.QtyPacked).IsRequired();
        builder.Property(po => po.Status).IsRequired();
        builder.Property(po => po.CreatedBy).IsRequired();

        builder.HasIndex(po => po.ProductionOrderNumber).IsUnique();

        builder.HasMany(po => po.CuttingOperations)
            .WithOne(co => co.ProductionOrder)
            .HasForeignKey(co => co.ProductionOrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(po => po.SewingOperations)
            .WithOne(so => so.ProductionOrder)
            .HasForeignKey(so => so.ProductionOrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(po => po.PackagingOperations)
            .WithOne(po2 => po2.ProductionOrder)
            .HasForeignKey(po2 => po2.ProductionOrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(po => po.InventoryMovements)
            .WithOne(im => im.ProductionOrder)
            .HasForeignKey(im => im.ProductionOrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(po => po.FinishedGoods)
            .WithOne(fg => fg.ProductionOrder)
            .HasForeignKey(fg => fg.ProductionOrderId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
