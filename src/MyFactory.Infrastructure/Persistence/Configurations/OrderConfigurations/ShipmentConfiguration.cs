using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFactory.Domain.Entities.Orders;

namespace MyFactory.Infrastructure.Persistence.Configurations.OrderConfigurations;

public class ShipmentConfiguration : IEntityTypeConfiguration<ShipmentEntity>
{
    public void Configure(EntityTypeBuilder<ShipmentEntity> builder)
    {
        builder.ToTable("SHIPMENTS");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.SalesOrderId).IsRequired();
        builder.Property(s => s.CustomerId).IsRequired();
        builder.Property(s => s.ShipmentDate).IsRequired();
        builder.Property(s => s.Status).IsRequired();
        builder.Property(s => s.CreatedBy).IsRequired();

        builder.HasMany(s => s.ShipmentItems)
            .WithOne(si => si.Shipment)
            .HasForeignKey(si => si.ShipmentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(s => s.ShipmentReturns)
            .WithOne(sr => sr.Shipment)
            .HasForeignKey(sr => sr.ShipmentId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
