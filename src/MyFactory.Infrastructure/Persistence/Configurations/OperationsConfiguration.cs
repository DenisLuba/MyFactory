using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFactory.Domain.Entities.Operations;

namespace MyFactory.Infrastructure.Persistence.Configurations;

public class OperationConfiguration : IEntityTypeConfiguration<Operation>
{
    public void Configure(EntityTypeBuilder<Operation> builder)
    {
        builder.ToTable("Operations");

        builder.HasKey(operation => operation.Id);

        builder.Property(operation => operation.Code)
            .IsRequired()
            .HasMaxLength(64);

        builder.Property(operation => operation.Name)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(operation => operation.DefaultTimeMinutes)
            .HasColumnType("decimal(10,2)")
            .IsRequired();

        builder.Property(operation => operation.DefaultCost)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(operation => operation.Type)
            .IsRequired()
            .HasMaxLength(128);

        builder.HasIndex(operation => operation.Code)
            .IsUnique();

        builder.Navigation(operation => operation.SpecificationOperations)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
