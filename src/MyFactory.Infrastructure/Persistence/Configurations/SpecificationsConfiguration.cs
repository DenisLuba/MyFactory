using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFactory.Domain.Entities.Specifications;
using MyFactory.Domain.ValueObjects;
using MyFactory.Infrastructure.Persistence.Constants;

namespace MyFactory.Infrastructure.Persistence.Configurations;

public class SpecificationConfiguration : IEntityTypeConfiguration<Specification>
{
    public void Configure(EntityTypeBuilder<Specification> builder)
    {
        builder.ToTable("Specifications");

        builder.HasKey(specification => specification.Id);

        builder.Property(specification => specification.Sku)
            .IsRequired()
            .HasMaxLength(FieldLengths.Code);

        builder.Property(specification => specification.Name)
            .IsRequired()
            .HasMaxLength(FieldLengths.Name);

        builder.Property(specification => specification.Description)
            .HasMaxLength(FieldLengths.Description);

        builder.Property(specification => specification.PlanPerHour)
            .HasColumnType(ColumnTypes.Quantity)
            .IsRequired();

        builder.Property(specification => specification.Status)
            .IsRequired()
            .HasMaxLength(FieldLengths.Status);

        builder.Property(specification => specification.Version)
            .IsRequired();

        builder.Property(specification => specification.CreatedAt)
            .IsRequired();

        builder.Navigation(specification => specification.BomItems)
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.Navigation(specification => specification.Operations)
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.HasIndex(specification => specification.Sku)
            .IsUnique();
    }
}

public class SpecificationBomItemConfiguration : IEntityTypeConfiguration<SpecificationBomItem>
{
    public void Configure(EntityTypeBuilder<SpecificationBomItem> builder)
    {
        builder.ToTable("SpecificationBomItems");

        builder.HasKey(item => item.Id);

        builder.Property(item => item.Quantity)
            .HasColumnType(ColumnTypes.Quantity)
            .IsRequired();

        builder.Property(item => item.Unit)
            .IsRequired()
            .HasMaxLength(FieldLengths.Unit);

        builder.HasOne(item => item.Specification)
            .WithMany(specification => specification.BomItems)
            .HasForeignKey(item => item.SpecificationId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(item => item.Material)
            .WithMany(material => material.BomItems)
            .HasForeignKey(item => item.MaterialId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.OwnsOne<Money>("_unitCost", owned =>
        {
            owned.Property(money => money.Amount)
                .HasColumnName("UnitCost")
                .HasColumnType(ColumnTypes.MonetaryHighPrecision);
        });

        builder.HasIndex(item => new { item.SpecificationId, item.MaterialId })
            .IsUnique();
    }
}

public class SpecificationOperationConfiguration : IEntityTypeConfiguration<SpecificationOperation>
{
    public void Configure(EntityTypeBuilder<SpecificationOperation> builder)
    {
        builder.ToTable("SpecificationOperations");

        builder.HasKey(operation => operation.Id);

        builder.Property(operation => operation.TimeMinutes)
            .HasColumnType(ColumnTypes.QuantitySmall)
            .IsRequired();

        builder.Property(operation => operation.OperationCost)
            .HasColumnType(ColumnTypes.Monetary)
            .IsRequired();

        builder.HasOne(operation => operation.Specification)
            .WithMany(specification => specification.Operations)
            .HasForeignKey(operation => operation.SpecificationId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(operation => operation.Operation)
            .WithMany(op => op.SpecificationOperations)
            .HasForeignKey(operation => operation.OperationId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(operation => operation.Workshop)
            .WithMany()
            .HasForeignKey(operation => operation.WorkshopId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(operation => new { operation.SpecificationId, operation.OperationId, operation.WorkshopId })
            .IsUnique();
    }
}
