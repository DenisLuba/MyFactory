using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFactory.Domain.Entities.Orders;

namespace MyFactory.Infrastructure.Persistence.Configurations.OrderConfigurations;

public class SalesOrderItemConfiguration : IEntityTypeConfiguration<SalesOrderItemEntity>
{
    public void Configure(EntityTypeBuilder<SalesOrderItemEntity> builder)
    {
        builder.ToTable("SALES_ORDER_ITEMS");

        builder.HasKey(soi => soi.Id);

        builder.Property(soi => soi.SalesOrderId).IsRequired();
        builder.Property(soi => soi.ProductId).IsRequired();
        builder.Property(soi => soi.QtyOrdered).IsRequired().HasColumnType("numeric(18,2)");
        builder.Property(soi => soi.QtyAllocated).IsRequired().HasColumnType("numeric(18,2)");
        builder.Property(soi => soi.QtyShipped).IsRequired().HasColumnType("numeric(18,2)");
        builder.Property(soi => soi.UnitPrice).HasColumnType("numeric(18,2)");
        builder.Property(soi => soi.Status).IsRequired();

        builder.HasMany(soi => soi.ProductionOrders)
            .WithOne(po => po.SalesOrderItem)
            .HasForeignKey(po => po.SalesOrderItemId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(soi => soi.ShipmentItems)
            .WithOne(si => si.SalesOrderItem)
            .HasForeignKey(si => si.SalesOrderItemId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(soi => soi.ShipmentReturnItems)
            .WithOne(sri => sri.SalesOrderItem)
            .HasForeignKey(sri => sri.SalesOrderItemId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
