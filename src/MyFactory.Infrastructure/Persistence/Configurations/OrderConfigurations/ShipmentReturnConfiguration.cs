using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFactory.Domain.Entities.Orders;

namespace MyFactory.Infrastructure.Persistence.Configurations.OrderConfigurations;

public class ShipmentReturnConfiguration : IEntityTypeConfiguration<ShipmentReturnEntity>
{
    public void Configure(EntityTypeBuilder<ShipmentReturnEntity> builder)
    {
        builder.ToTable("SHIPMENT_RETURNS");

        builder.HasKey(sr => sr.Id);

        builder.Property(sr => sr.ShipmentId);
        builder.Property(sr => sr.SalesOrderId).IsRequired();
        builder.Property(sr => sr.CustomerId).IsRequired();
        builder.Property(sr => sr.ReturnDate).IsRequired();
        builder.Property(sr => sr.Reason).HasMaxLength(500);
        builder.Property(sr => sr.Status).IsRequired();
        builder.Property(sr => sr.CreatedBy).IsRequired();

        builder.HasMany(sr => sr.ShipmentReturnItems)
            .WithOne(sri => sri.ShipmentReturn)
            .HasForeignKey(sri => sri.ShipmentReturnId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
