using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFactory.Domain.Entities.Sales;

namespace MyFactory.Infrastructure.Persistence.Configurations;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("Customers");

        builder.HasKey(customer => customer.Id);

        builder.Property(customer => customer.Name)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(customer => customer.Contact)
            .IsRequired()
            .HasMaxLength(256);

        builder.HasIndex(customer => customer.Name)
            .IsUnique();
    }
}

public class ShipmentConfiguration : IEntityTypeConfiguration<Shipment>
{
    public void Configure(EntityTypeBuilder<Shipment> builder)
    {
        builder.ToTable("Shipments");

        builder.HasKey(shipment => shipment.Id);

        builder.Property(shipment => shipment.ShipmentNumber)
            .IsRequired()
            .HasMaxLength(64);

        builder.Property(shipment => shipment.ShipmentDate)
            .IsRequired();

        builder.Property(shipment => shipment.Status)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(shipment => shipment.TotalAmount)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.HasOne(shipment => shipment.Customer)
            .WithMany()
            .HasForeignKey(shipment => shipment.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Navigation(shipment => shipment.Items)
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.HasIndex(shipment => shipment.ShipmentNumber)
            .IsUnique();
    }
}

public class ShipmentItemConfiguration : IEntityTypeConfiguration<ShipmentItem>
{
    public void Configure(EntityTypeBuilder<ShipmentItem> builder)
    {
        builder.ToTable("ShipmentItems");

        builder.HasKey(item => item.Id);

        builder.Property(item => item.Quantity)
            .HasColumnType("decimal(18,3)")
            .IsRequired();

        builder.Property(item => item.UnitPrice)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.HasOne(item => item.Shipment)
            .WithMany(shipment => shipment.Items)
            .HasForeignKey(item => item.ShipmentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(item => item.Specification)
            .WithMany()
            .HasForeignKey(item => item.SpecificationId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class CustomerReturnConfiguration : IEntityTypeConfiguration<CustomerReturn>
{
    public void Configure(EntityTypeBuilder<CustomerReturn> builder)
    {
        builder.ToTable("CustomerReturns");

        builder.HasKey(returnEntity => returnEntity.Id);

        builder.Property(returnEntity => returnEntity.ReturnNumber)
            .IsRequired()
            .HasMaxLength(64);

        builder.Property(returnEntity => returnEntity.ReturnDate)
            .IsRequired();

        builder.Property(returnEntity => returnEntity.Reason)
            .IsRequired()
            .HasMaxLength(512);

        builder.Property(returnEntity => returnEntity.Status)
            .HasConversion<int>()
            .IsRequired();

        builder.HasOne(returnEntity => returnEntity.Customer)
            .WithMany()
            .HasForeignKey(returnEntity => returnEntity.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Navigation(returnEntity => returnEntity.Items)
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.HasIndex(returnEntity => returnEntity.ReturnNumber)
            .IsUnique();
    }
}

public class CustomerReturnItemConfiguration : IEntityTypeConfiguration<CustomerReturnItem>
{
    public void Configure(EntityTypeBuilder<CustomerReturnItem> builder)
    {
        builder.ToTable("CustomerReturnItems");

        builder.HasKey(item => item.Id);

        builder.Property(item => item.Quantity)
            .HasColumnType("decimal(18,3)")
            .IsRequired();

        builder.Property(item => item.Disposition)
            .IsRequired()
            .HasMaxLength(256);

        builder.HasOne(item => item.CustomerReturn)
            .WithMany(returnEntity => returnEntity.Items)
            .HasForeignKey(item => item.CustomerReturnId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(item => item.Specification)
            .WithMany()
            .HasForeignKey(item => item.SpecificationId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
