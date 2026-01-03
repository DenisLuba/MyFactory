using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFactory.Domain.Entities.Orders;

namespace MyFactory.Infrastructure.Persistence.Configurations.OrderConfigurations;

public class SalesOrderConfiguration : IEntityTypeConfiguration<SalesOrderEntity>
{
    public void Configure(EntityTypeBuilder<SalesOrderEntity> builder)
    {
        builder.ToTable("SALES_ORDERS");

        builder.HasKey(so => so.Id);

        builder.Property(so => so.OrderNumber)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(so => so.CustomerId).IsRequired();
        builder.Property(so => so.OrderDate).IsRequired();
        builder.Property(so => so.RequiredByDate);
        builder.Property(so => so.Status).IsRequired();
        builder.Property(so => so.CreatedBy).IsRequired();

        builder.HasIndex(so => so.OrderNumber).IsUnique();

        builder.HasMany(so => so.SalesOrderItems)
            .WithOne(soi => soi.SalesOrder)
            .HasForeignKey(soi => soi.SalesOrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(so => so.Shipments)
            .WithOne(s => s.SalesOrder)
            .HasForeignKey(s => s.SalesOrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(so => so.ShipmentReturns)
            .WithOne(sr => sr.SalesOrder)
            .HasForeignKey(sr => sr.SalesOrderId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
