using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFactory.Domain.Entities.Production;

namespace MyFactory.Infrastructure.Persistence.Configurations.ProductionConfigurations;

public class PackagingOperationConfiguration : IEntityTypeConfiguration<PackagingOperationEntity>
{
    public void Configure(EntityTypeBuilder<PackagingOperationEntity> builder)
    {
        builder.ToTable("PACKAGING_OPERATIONS");

        builder.HasKey(po => po.Id);

        builder.Property(po => po.ProductionOrderId).IsRequired();
        builder.Property(po => po.EmployeeId).IsRequired();
        builder.Property(po => po.QtyPlanned).IsRequired();
        builder.Property(po => po.QtyPacked).IsRequired();
        builder.Property(po => po.OperationDate).IsRequired();
    }
}
