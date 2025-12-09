using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFactory.Domain.Entities.Sales;
using MyFactory.Infrastructure.Persistence.Constants;

namespace MyFactory.Infrastructure.Persistence.Configurations;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("Customers");

        builder.HasKey(customer => customer.Id);

        builder.Property(customer => customer.Name)
            .IsRequired()
            .HasMaxLength(FieldLengths.Name);

        builder.Property(customer => customer.Contact)
            .IsRequired()
            .HasMaxLength(FieldLengths.Contact);

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
            .HasMaxLength(FieldLengths.Code);

        builder.Property(shipment => shipment.ShipmentDate)
            .IsRequired();

        builder.Property(shipment => shipment.Status)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(shipment => shipment.TotalAmount)
            .HasColumnType(ColumnTypes.Monetary)
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
            .HasColumnType(ColumnTypes.Quantity)
            .IsRequired();

        builder.Property(item => item.UnitPrice)
            .HasColumnType(ColumnTypes.Monetary)
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
            .HasMaxLength(FieldLengths.Code);

        builder.Property(returnEntity => returnEntity.ReturnDate)
            .IsRequired();

        builder.Property(returnEntity => returnEntity.Reason)
            .IsRequired()
            .HasMaxLength(FieldLengths.Notes);

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
            .HasColumnType(ColumnTypes.Quantity)
            .IsRequired();

        builder.Property(item => item.Disposition)
            .IsRequired()
            .HasMaxLength(FieldLengths.Name);

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
