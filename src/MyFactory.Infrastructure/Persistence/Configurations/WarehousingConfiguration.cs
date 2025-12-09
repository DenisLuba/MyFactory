using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFactory.Domain.Entities.Warehousing;
using MyFactory.Infrastructure.Persistence.Constants;

namespace MyFactory.Infrastructure.Persistence.Configurations;

public class WarehouseConfiguration : IEntityTypeConfiguration<Warehouse>
{
    public void Configure(EntityTypeBuilder<Warehouse> builder)
    {
        builder.ToTable("Warehouses");

        builder.HasKey(warehouse => warehouse.Id);

        builder.Property(warehouse => warehouse.Name)
            .IsRequired()
            .HasMaxLength(FieldLengths.Name);

        builder.Property(warehouse => warehouse.Type)
            .IsRequired()
            .HasMaxLength(FieldLengths.ShortText);

        builder.Property(warehouse => warehouse.Location)
            .IsRequired()
            .HasMaxLength(FieldLengths.Name);

        builder.Navigation(warehouse => warehouse.InventoryItems)
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.HasIndex(warehouse => warehouse.Name)
            .IsUnique();
    }
}

public class InventoryItemConfiguration : IEntityTypeConfiguration<InventoryItem>
{
    public void Configure(EntityTypeBuilder<InventoryItem> builder)
    {
        builder.ToTable("InventoryItems");

        builder.HasKey(item => item.Id);

        builder.Property(item => item.Quantity)
            .HasColumnType(ColumnTypes.Quantity)
            .IsRequired();

        builder.Property(item => item.AveragePrice)
            .HasColumnType(ColumnTypes.MonetaryHighPrecision)
            .IsRequired();

        builder.Property(item => item.ReservedQuantity)
            .HasColumnType(ColumnTypes.Quantity)
            .IsRequired();

        builder.HasOne(item => item.Warehouse)
            .WithMany(warehouse => warehouse.InventoryItems)
            .HasForeignKey(item => item.WarehouseId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(item => item.Material)
            .WithMany(material => material.InventoryItems)
            .HasForeignKey(item => item.MaterialId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(item => new { item.WarehouseId, item.MaterialId })
            .IsUnique();
    }
}

public class InventoryReceiptConfiguration : IEntityTypeConfiguration<InventoryReceipt>
{
    public void Configure(EntityTypeBuilder<InventoryReceipt> builder)
    {
        builder.ToTable("InventoryReceipts");

        builder.HasKey(receipt => receipt.Id);

        builder.Property(receipt => receipt.ReceiptNumber)
            .IsRequired()
            .HasMaxLength(FieldLengths.Code);

        builder.Property(receipt => receipt.ReceiptDate)
            .IsRequired();

        builder.Property(receipt => receipt.TotalAmount)
            .HasColumnType(ColumnTypes.Monetary)
            .IsRequired();

        builder.Property(receipt => receipt.Status)
            .HasConversion<int>()
            .IsRequired();

        builder.HasOne(receipt => receipt.Supplier)
            .WithMany(supplier => supplier.Receipts)
            .HasForeignKey(receipt => receipt.SupplierId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Navigation(receipt => receipt.Items)
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.HasIndex(receipt => receipt.ReceiptNumber)
            .IsUnique();
    }
}

public class InventoryReceiptItemConfiguration : IEntityTypeConfiguration<InventoryReceiptItem>
{
    public void Configure(EntityTypeBuilder<InventoryReceiptItem> builder)
    {
        builder.ToTable("InventoryReceiptItems");

        builder.HasKey(item => item.Id);

        builder.Property(item => item.Quantity)
            .HasColumnType(ColumnTypes.Quantity)
            .IsRequired();

        builder.Property(item => item.UnitPrice)
            .HasColumnType(ColumnTypes.MonetaryHighPrecision)
            .IsRequired();

        builder.HasOne(item => item.InventoryReceipt)
            .WithMany(receipt => receipt.Items)
            .HasForeignKey(item => item.InventoryReceiptId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(item => item.Material)
            .WithMany(material => material.ReceiptItems)
            .HasForeignKey(item => item.MaterialId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(item => item.InventoryItem)
            .WithMany()
            .HasForeignKey(item => item.InventoryItemId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}

public class PurchaseRequestConfiguration : IEntityTypeConfiguration<PurchaseRequest>
{
    public void Configure(EntityTypeBuilder<PurchaseRequest> builder)
    {
        builder.ToTable("PurchaseRequests");

        builder.HasKey(request => request.Id);

        builder.Property(request => request.PrNumber)
            .IsRequired()
            .HasMaxLength(FieldLengths.Code);

        builder.Property(request => request.CreatedAt)
            .IsRequired();

        builder.Property(request => request.Status)
            .HasConversion<int>()
            .IsRequired();

        builder.Navigation(request => request.Items)
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.HasIndex(request => request.PrNumber)
            .IsUnique();
    }
}

public class PurchaseRequestItemConfiguration : IEntityTypeConfiguration<PurchaseRequestItem>
{
    public void Configure(EntityTypeBuilder<PurchaseRequestItem> builder)
    {
        builder.ToTable("PurchaseRequestItems");

        builder.HasKey(item => item.Id);

        builder.Property(item => item.Quantity)
            .HasColumnType(ColumnTypes.Quantity)
            .IsRequired();

        builder.HasOne(item => item.PurchaseRequest)
            .WithMany(request => request.Items)
            .HasForeignKey(item => item.PurchaseRequestId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(item => item.Material)
            .WithMany(material => material.PurchaseRequestItems)
            .HasForeignKey(item => item.MaterialId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(item => new { item.PurchaseRequestId, item.MaterialId })
            .IsUnique();
    }
}
