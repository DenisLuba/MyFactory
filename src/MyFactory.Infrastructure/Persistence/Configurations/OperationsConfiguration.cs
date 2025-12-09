using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFactory.Domain.Entities.Operations;
using MyFactory.Infrastructure.Persistence.Constants;

namespace MyFactory.Infrastructure.Persistence.Configurations;

public class OperationConfiguration : IEntityTypeConfiguration<Operation>
{
    public void Configure(EntityTypeBuilder<Operation> builder)
    {
        builder.ToTable("Operations");

        builder.HasKey(operation => operation.Id);

        builder.Property(operation => operation.Code)
            .IsRequired()
            .HasMaxLength(FieldLengths.Code);

        builder.Property(operation => operation.Name)
            .IsRequired()
            .HasMaxLength(FieldLengths.Name);

        builder.Property(operation => operation.DefaultTimeMinutes)
            .HasColumnType(ColumnTypes.QuantitySmall)
            .IsRequired();

        builder.Property(operation => operation.DefaultCost)
            .HasColumnType(ColumnTypes.Monetary)
            .IsRequired();

        builder.Property(operation => operation.Type)
            .IsRequired()
            .HasMaxLength(FieldLengths.ShortText);

        builder.HasIndex(operation => operation.Code)
            .IsUnique();

        builder.Navigation(operation => operation.SpecificationOperations)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
