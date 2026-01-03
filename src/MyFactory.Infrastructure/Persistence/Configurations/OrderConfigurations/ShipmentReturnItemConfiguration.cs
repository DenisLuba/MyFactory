using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFactory.Domain.Entities.Orders;

namespace MyFactory.Infrastructure.Persistence.Configurations.OrderConfigurations;

public class ShipmentReturnItemConfiguration : IEntityTypeConfiguration<ShipmentReturnItemEntity>
{
    public void Configure(EntityTypeBuilder<ShipmentReturnItemEntity> builder)
    {
        builder.ToTable("SHIPMENT_RETURN_ITEMS");

        builder.HasKey(sri => sri.Id);

        builder.Property(sri => sri.ShipmentReturnId).IsRequired();
        builder.Property(sri => sri.ShipmentItemId);
        builder.Property(sri => sri.SalesOrderItemId).IsRequired();
        builder.Property(sri => sri.ProductId).IsRequired();
        builder.Property(sri => sri.WarehouseId).IsRequired();
        builder.Property(sri => sri.Qty).IsRequired();
        builder.Property(sri => sri.UnitPrice).IsRequired().HasColumnType("numeric(18,2)");
        builder.Property(sri => sri.Condition).IsRequired();
    }
}
