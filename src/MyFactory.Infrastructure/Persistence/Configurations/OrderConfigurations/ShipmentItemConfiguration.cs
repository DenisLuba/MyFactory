using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFactory.Domain.Entities.Orders;

namespace MyFactory.Infrastructure.Persistence.Configurations.OrderConfigurations;

public class ShipmentItemConfiguration : IEntityTypeConfiguration<ShipmentItemEntity>
{
    public void Configure(EntityTypeBuilder<ShipmentItemEntity> builder)
    {
        builder.ToTable("SHIPMENT_ITEMS");

        builder.HasKey(si => si.Id);

        builder.Property(si => si.ShipmentId).IsRequired();
        builder.Property(si => si.SalesOrderItemId).IsRequired();
        builder.Property(si => si.WarehouseId).IsRequired();
        builder.Property(si => si.ProductId).IsRequired();
        builder.Property(si => si.Qty).IsRequired().HasColumnType("numeric(18,2)");
        builder.Property(si => si.UnitPrice).IsRequired().HasColumnType("numeric(18,2)");

        builder.HasMany(si => si.ShipmentReturnItems)
            .WithOne(sri => sri.ShipmentItem)
            .HasForeignKey(sri => sri.ShipmentItemId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
